using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetScape.DanielPellanda
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

            this.playerEntity = new Player(this);

            this.spawner = new Generator(this.Entities, ALogics.SpawnInterval);
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
            this.GetEntitiesCleaner().Invoke(t => t != EntityType.PLAYER, e => true);
            this.playerEntity.Reset();
        }

        private void UpdateCleaner()
        {
            if (ALogics.FrameTime % GameWindow.FPS_LIMIT * ALogics.CleanInterval == 0)
            {
                this.GetEntitiesCleaner().Invoke(t => t.IsGenerableEntity(), e => e.IsOnClearArea());
            }
        }

        private void UpdateDifficulty()
        {
            ALogics.DifficultyLevel = this.playerEntity.CurrentScore() / ALogics.IncreaseDiffPerScore + 1;
        }

        private void SetGameState(GameState gs)
        {
            if (this.gameState != gs)
            {
                switch (gs)
                {
                    case GameState.INGAME:
                        if (this.gameState == GameState.ENDGAME)
                        { // RETRY
                            this.ResetGame();
                        }
                        else if (this.gameState == GameState.MENU)
                        { // START
                            this.ResetGame();
                            this.entities[EntityType.PLAYER].Add(playerEntity);
                        }
                        this.spawner.Resume();
                        break;
                    case GameState.MENU:
                        if (this.gameState == GameState.PAUSED)
                        {
                            this.spawner.Stop();
                            return;
                        }
                        this.GetEntitiesCleaner().Invoke(t => true, e => true);
                        break;
                    case GameState.ENDGAME:
                        this.spawner.Stop();
                        break;
                    case GameState.PAUSED:
                        if (this.gameState != GameState.INGAME)
                        {
                            return;
                        }
                        this.spawner.Pause();
                        break;
                    default:
                        break;
                }
                this.gameState = gs;
            }
        }
        public override void UpdateAll()
        {
            switch (this.gameState)
            {
                case GameState.EXIT:
                    this.spawner.Terminate();
                    break;
                case GameState.ENDGAME:
                    this.playerEntity.Update();
                    break;
                case GameState.INGAME:
                    if (this.playerEntity.hasDied())
                    {
                        this.SetGameState(GameState.ENDGAME);
                        break;
                    }
                    this.UpdateDifficulty();
                    this.UpdateCleaner();

                    this.spawner.Mutex.WaitOne();
                    foreach (ISet<IEntity> sets in entities.Keys)
                    {
                        foreach (IEntity entity in sets)
                        {
                            entity.Update();
                        }    
                    }
                    this.spawner.Mutex.ReleaseMutex();
                    break;
                default:
                    break;
            }
            ALogics.UpdateTimer();
        }
    }
}
