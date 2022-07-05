using DanielPellanda.game.frame;
using DanielPellanda.game.logics.entities;
using DanielPellanda.game.logics.entities.player;
using DanielPellanda.game.logics.generator;
using DanielPellanda.game.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanielPellanda.game.logics
{
    public delegate void Cleaner(Predicate<EntityType> typeCondition, Predicate<IEntity> entityCondition);

    public class Logics : ALogics
    {
        private readonly IDictionary<EntityType, ISet<IEntity>> entities = new Dictionary<EntityType, ISet<IEntity>>();
        private readonly IPlayer playerEntity;
        private IGenerator spawner;

        private GameState gameState;

        public IDictionary<EntityType, ISet<IEntity>> Entities { get => entities; }

        public Logics() : base()
        {

            foreach (EntityType e in EntityType.ALL_ENTITY_TYPE)
            {
                entities.Add(e, new HashSet<IEntity>());
            }

            playerEntity = new Player(this);

            spawner = new Generator(Entities, SpawnInterval);
            //this.initializeSpawner();
        }

        /*
        private void initializeSpawner()
        {

            this.spawner.setMissileCreator(p-> new MissileInstance(this, p,
                    this.playerEntity, super.getEntityMovementInfo(EntityType.MISSILE)));
            this.spawner.setZapperBaseCreator(p-> new ZapperBaseInstance(this, p,
                    super.getEntityMovementInfo(EntityType.ZAPPERBASE)));
            this.spawner.setZapperRayCreator((b, p)-> new ZapperRayInstance(this, p, b.getX(), b.getY()));
            this.spawner.setShieldCreator(p-> new ShieldInstance(this, p,
                    this.playerEntity, super.getEntityMovementInfo(EntityType.SHIELD)));
            this.spawner.setTeleportCreator(p-> new TeleportInstance(this, p,
                    this.playerEntity, super.getEntityMovementInfo(EntityType.TELEPORT)));
            this.spawner.setCoinCreator(p-> new CoinInstance(this, p,
                    this.playerEntity, super.getEntityMovementInfo(EntityType.COIN)));

            try
            {
                this.spawner.initialize();
            }
            catch (FileNotFoundException e)
            {
                JOptionPane.showMessageDialog((Component)GameHandler.GAME_WINDOW,
                        "Tiles information file cannot be found.\n\nDetails:\n"
                        + e.getMessage(), "Error", JOptionPane.ERROR_MESSAGE);
                e.printStackTrace();
            }
            catch (JsonException e)
            {
                JOptionPane.showMessageDialog((Component)GameHandler.GAME_WINDOW,
                        "An error occured while trying to load tiles.\n\nDetails:\n"
                        + e.getMessage(), "Error", JOptionPane.ERROR_MESSAGE);
                e.printStackTrace();
            }
            catch (FormatException e)
            {
                JOptionPane.showMessageDialog((Component)GameHandler.GAME_WINDOW,
                        "Tiles information file has an incorrect format.\n\nDetails:\n"
                        + e.getMessage(), "Error", JOptionPane.ERROR_MESSAGE);
                e.printStackTrace();
            }
        }*/

        public override Cleaner GetEntitiesCleaner()
        {
            return delegate (Predicate<EntityType> typeCondition, Predicate<IEntity> entityCondition)
            {
                spawner.Mutex.WaitOne();

                var typesToClean =
                    from s in entities
                    where typeCondition.Invoke(s.Key)
                    select s.Value;
                foreach (ISet<IEntity> s in typesToClean)
                {
                    var entitiesToClean =
                        from e in s
                        where entityCondition.Invoke(e)
                        select e;
                    foreach (IEntity e in entitiesToClean)
                    {
                        e.Reset();
                        s.Remove(e);
                    }
                }
                //GameWindow.GAME_DEBUGGER.printLog(Debugger.Option.LOG_CLEAN, "cleaned::" + e.toString()

                spawner.Mutex.ReleaseMutex();
            };
        }

        private void ResetGame()
        {
            GetEntitiesCleaner().Invoke(t => t != EntityType.PLAYER, e => true);
            playerEntity.Reset();
        }

        private void UpdateCleaner()
        {
            if (FrameTime % GameWindow.FPS_LIMIT * CleanInterval == 0)
            {
                GetEntitiesCleaner().Invoke(t => t.IsGenerableEntity(), e => e.IsOnClearArea());
            }
        }

        private void UpdateDifficulty()
        {
            DifficultyLevel = playerEntity.CurrentScore() / IncreaseDiffPerScore + 1;
        }

        private void SetGameState(GameState gs)
        {
            if (gameState != gs)
            {
                switch (gs)
                {
                    case GameState.INGAME:
                        if (gameState == GameState.ENDGAME)
                        { // RETRY
                            ResetGame();
                        }
                        else if (gameState == GameState.MENU)
                        { // START
                            ResetGame();
                            entities[EntityType.PLAYER].Add(playerEntity);
                        }
                        spawner.Resume();
                        break;
                    case GameState.MENU:
                        if (gameState == GameState.PAUSED)
                        {
                            spawner.Stop();
                            return;
                        }
                        GetEntitiesCleaner().Invoke(t => true, e => true);
                        break;
                    case GameState.ENDGAME:
                        spawner.Stop();
                        break;
                    case GameState.PAUSED:
                        if (gameState != GameState.INGAME)
                        {
                            return;
                        }
                        spawner.Pause();
                        break;
                    default:
                        break;
                }
                gameState = gs;
            }
        }
        public override void UpdateAll()
        {
            switch (gameState)
            {
                case GameState.EXIT:
                    spawner.Terminate();
                    break;
                case GameState.ENDGAME:
                    playerEntity.Update();
                    break;
                case GameState.INGAME:
                    if (playerEntity.hasDied())
                    {
                        SetGameState(GameState.ENDGAME);
                        break;
                    }
                    UpdateDifficulty();
                    UpdateCleaner();

                    spawner.Mutex.WaitOne();
                    foreach (ISet<IEntity> sets in entities.Keys)
                    {
                        foreach (IEntity entity in sets)
                        {
                            entity.Update();
                        }
                    }
                    spawner.Mutex.ReleaseMutex();
                    break;
                default:
                    break;
            }
            UpdateTimer();
        }
    }
}
