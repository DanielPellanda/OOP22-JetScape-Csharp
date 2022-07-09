using JetScape.game.logics.entities;
using JetScape.game.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;

using JetScape.game.frame;

namespace JetScape.game.logics.generator
{
    public class Generator : IGenerator
    { 
        public MissileFactory CreateMissile { private get; set; } = null;
        public ShieldFactory CreateShield { private get; set; } = null;
        internal IDictionary<EntityType, ISet<IEntity>> Entities { get; private set; }

        private const int COINS_ODDS = 50;
        private const int MISSILE_ODDS = 25;
        private const int POWERUP_ODDS = 5;

        internal const long INTERVAL_DECREASE_DIFF = 40;
        internal const long MINIMAL_INTERVAL = 20;
        private int _tileSize;

        private Random _rng = new Random();

        internal long Interval { get; private set; }
        internal long SleepInterval { get; set; }

        public bool IsRunning { get => GeneratorThread.IsRunning; }
        public bool IsWaiting { get => GeneratorThread.IsWaiting; }

        public Generator(IDictionary<EntityType, ISet<IEntity>> entities, double interval)
        {
            this.Entities = entities;
            this._tileSize = GameWindow.ScreenInfo.TileSize;
            this.Interval = (long)(interval * 1000 + INTERVAL_DECREASE_DIFF);
            this.SleepInterval = this.Interval;
        }

        internal void SpawnTile()
        {
            int rollOdds = MISSILE_ODDS + POWERUP_ODDS;
            int pick = _rng.Next(rollOdds);

            if (pick <= POWERUP_ODDS)
            {
                if (CreateShield == null) return;

                Entities[EntityType.SHIELD].Add(CreateShield.Invoke(new Point(GameWindow.ScreenInfo.Width, GameWindow.ScreenInfo.Height / 2)));
            }
            else
            {
                if (CreateMissile == null) return;

                Entities[EntityType.MISSILE].Add(CreateMissile.Invoke(new Point(GameWindow.ScreenInfo.Width, GameWindow.ScreenInfo.Height / 2)));
            }
        }

        public void Initialize() 
        {
            GeneratorThread.GeneratorToExecute = this;
        }

        public void Start() => GeneratorThread.Start();

        public void Terminate() => GeneratorThread.Terminate();

        public void Stop() => GeneratorThread.Stop();

        public void Pause() => GeneratorThread.Pause();

        public void Resume() => GeneratorThread.Resume();
    }

    internal static class GeneratorThread
    {
        private static long _systemTimeBeforeSleep;
        private static long _systemTimeAfterPaused;
        private static long _remainingTimeToSleep;
        private static long _sleepTimeLeft;

        private static Thread TGenerator { get; } = new Thread(new ThreadStart(Run));
        internal static Generator GeneratorToExecute { get; set; } = null;

        public static bool IsRunning { get; internal set; } = false;
        public static bool IsWaiting { get; internal set; } = false;

        public static void Start()
        {
            if (GeneratorToExecute == null) return;

            lock (TGenerator)
            {
                if (!IsRunning)
                {
                    IsRunning = true;
                    IsWaiting = true;
                    TGenerator.Start();
                }
            }
        }

        public static void Terminate()
        {
            if (GeneratorToExecute == null) return;

            lock (TGenerator)
            {
                IsRunning = false;
            }
            Resume();
        }

        public static void Stop()
        {
            if (GeneratorToExecute == null) return;

            lock (TGenerator)
            {
                IsWaiting = true;

                _remainingTimeToSleep = 0;
            }
        }

        public static void Pause()
        {
            if (GeneratorToExecute == null) return;

            lock (TGenerator)
            {
                IsWaiting = true;

                long timePassed = GameWindow.NanoTimeFromEpoch() / 1000000 - _systemTimeBeforeSleep;
                _remainingTimeToSleep = GeneratorToExecute.SleepInterval - timePassed;
            }
        }

        public static void Resume()
        {
            if (GeneratorToExecute == null) return;

            lock (TGenerator)
            {
                if (IsWaiting)
                {
                    IsWaiting = false;
                    Monitor.PulseAll(TGenerator);
                    long timePassed = GameWindow.NanoTimeFromEpoch()
                            / 1000000 - _systemTimeBeforeSleep;
                    _sleepTimeLeft = GeneratorToExecute.SleepInterval - timePassed > 0
                            ? GeneratorToExecute.SleepInterval - timePassed : 0;
                    _remainingTimeToSleep = timePassed < GeneratorToExecute.SleepInterval
                            ? _remainingTimeToSleep - _sleepTimeLeft
                            : _remainingTimeToSleep;
                    _systemTimeAfterPaused = GameWindow.NanoTimeFromEpoch() / 1000000;
                }
            }
        }

        private static void Run()
        {
            if (GeneratorToExecute == null) return;

            long minimum = (GeneratorToExecute.Interval - Generator.INTERVAL_DECREASE_DIFF)
                    * Generator.MINIMAL_INTERVAL / 100;

            while (TGenerator.IsAlive && IsRunning)
            {

                lock (TGenerator)
                {
                    while (IsWaiting)
                    {
                        Monitor.Wait(TGenerator);
                        if (!IsRunning)
                        {
                            continue;
                        }
                    }
                    Thread.Sleep((int)_remainingTimeToSleep);
                    _remainingTimeToSleep = 0;
                    _sleepTimeLeft = 0;

                    lock (GeneratorToExecute.Entities)
                    {
                        GeneratorToExecute.SpawnTile();
                    }

                    _systemTimeBeforeSleep = GameWindow.NanoTimeFromEpoch() / 1000000;
                    GeneratorToExecute.SleepInterval = GeneratorToExecute.Interval - Generator.INTERVAL_DECREASE_DIFF * ALogics.DifficultyLevel > minimum
                            ? GeneratorToExecute.Interval - Generator.INTERVAL_DECREASE_DIFF * ALogics.DifficultyLevel
                            : minimum;
                }
                Thread.Sleep((int)GeneratorToExecute.SleepInterval);
            }
        }
    }
}
