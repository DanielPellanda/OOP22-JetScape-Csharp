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

namespace JetScape.game.logics
{
    public delegate void Cleaner(Predicate<EntityType> typeCondition, Predicate<IEntity> entityCondition);

    public class Logics : ALogics, ILogics
    {
        private readonly IDictionary<EntityType, ISet<IEntity>> _entities = new Dictionary<EntityType, ISet<IEntity>>();
        private readonly IPlayer _playerEntity;
        private IGenerator _spawner;

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
                _spawner.Mutex.WaitOne();

                var typesToClean =
                    from s in _entities
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

                _spawner.Mutex.ReleaseMutex();
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
                        { // RETRY
                            ResetGame();
                        }
                        else if (_gameState == GameState.MENU)
                        { // START
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

                    _spawner.Mutex.WaitOne();
                    foreach (ISet<IEntity> sets in _entities.Keys)
                    {
                        foreach (IEntity entity in sets)
                        {
                            entity.Update();
                        }
                    }
                    _spawner.Mutex.ReleaseMutex();
                    break;
                default:
                    break;
            }
            UpdateTimer();
        }
    }
}
