using JetScape.game.frame;
using JetScape.game.logics.entities;
using JetScape.game.logics.entities.player;
using JetScape.game.logics.generator;
using JetScape.game.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using JetScape.game.logics.entities.obstacles;
using JetScape.game.logics.entities.obstacles.missile;
using JetScape.game.logics.entities.pickups;
using JetScape.game.logics.entities.pickups.shield;

namespace JetScape.game.logics
{
    public delegate void Cleaner(Predicate<EntityType> typeCondition, Predicate<IEntity> entityCondition);
    public delegate IObstacle MissileFactory(Point position);
    public delegate IPickup ShieldFactory(Point position);

    public class Logics : ALogics, ILogics
    {
        private readonly IDictionary<EntityType, ISet<IEntity>> _entities = new Dictionary<EntityType, ISet<IEntity>>();
        private readonly IPlayer _playerEntity;
        private readonly IGenerator _spawner;

        private GameState _gameState;

        // Property added for testing porposes
        public GameState GameState { get => _gameState; set => SetGameState(value); }
        public IDictionary<EntityType, ISet<IEntity>> Entities { get => _entities; }

        public Logics() : base()
        {

            foreach (EntityType e in EntityType.ALL_ENTITY_TYPE)
            {
                _entities.Add(e, new HashSet<IEntity>());
            }

            _playerEntity = new Player(this);

            _spawner = new Generator(Entities, SpawnInterval);
            this.InitializeSpawner();
        }

        
        private void InitializeSpawner()
        {

            _spawner.CreateMissile = p => new Missile(this, p,
                    _playerEntity, GetEntityMovementInfo(EntityType.MISSILE));
            _spawner.CreateShield = p => new Shield(this, p,
                    _playerEntity, GetEntityMovementInfo(EntityType.SHIELD));

            _spawner.Initialize();
        }

        public override Cleaner GetEntitiesCleaner()
        {
            return delegate (Predicate<EntityType> typeCondition, Predicate<IEntity> entityCondition)
            {
                lock (Entities)
                {
                    var typesToClean =
                        from s in _entities
                        where typeCondition.Invoke(s.Key)
                        select s.Value;
                    foreach (ISet<IEntity> s in typesToClean)
                    {
                        IList<IEntity> entitiesToClean = new List<IEntity>(s.Where<IEntity>(e => entityCondition.Invoke(e)));
                        foreach (IEntity e in entitiesToClean)
                        {
                            e.Reset();
                            s.Remove(e);
                        }
                    }
                }
            };
        }

        private void ResetGame()
        {
            GetEntitiesCleaner().Invoke(t => t != EntityType.PLAYER, e => true);
            _playerEntity.Reset();
        }

        private void UpdateCleaner()
        {
            if (FrameTime % GameWindow.FPS_LIMIT * CleanInterval == 0)
            {
                GetEntitiesCleaner().Invoke(t => t.IsGenerableEntity(), e => e.IsOnClearArea);
            }
        }

        private void UpdateDifficulty() => DifficultyLevel = _playerEntity.CurrentScore / IncreaseDiffPerScore + 1;

        private void SetGameState(GameState gs)
        {
            if (_gameState != gs)
            {
                switch (gs)
                {
                    case GameState.INGAME:
                        if (_gameState == GameState.ENDGAME)
                        {
                            ResetGame();
                        }
                        else if (_gameState == GameState.MENU)
                        {
                            ResetGame();
                            _entities[EntityType.PLAYER].Add(_playerEntity);
                        }
                        _spawner.Resume();
                        break;
                    case GameState.MENU:
                        if (_gameState == GameState.PAUSED)
                        {
                            _spawner.Stop();
                            return;
                        }
                        GetEntitiesCleaner().Invoke(t => true, e => true);
                        break;
                    case GameState.ENDGAME:
                        _spawner.Stop();
                        break;
                    case GameState.PAUSED:
                        if (_gameState != GameState.INGAME)
                        {
                            return;
                        }
                        _spawner.Pause();
                        break;
                    default:
                        break;
                }
                _gameState = gs;
            }
        }
        public override void UpdateAll()
        {
            switch (_gameState)
            {
                case GameState.EXIT:
                    _spawner.Terminate();
                    break;
                case GameState.ENDGAME:
                    _playerEntity.Update();
                    break;
                case GameState.INGAME:
                    if (_playerEntity.HasDied)
                    {
                        GameState = GameState.ENDGAME;
                        break;
                    }
                    UpdateDifficulty();
                    UpdateCleaner();

                    lock (Entities)
                    {
                        foreach (ISet<IEntity> sets in _entities.Values)
                        {
                            foreach (IEntity entity in sets)
                            {
                                entity.Update();
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            UpdateTimer();
        }
    }
}
