using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JetScape.DanielPellanda
{ 
    public abstract class ALogics : ILogics
    {

        private static int frameTime;
        private static int difficultyLevel = 1;

        private const int INCREASE_DIFF_PER_SCORE = 250;

        private const double SPAWN_INTERVAL = 3.3;
        private const double CLEAN_INTERVAL = 5.0;

        public static int FrameTime { get => frameTime; }
        public static int DifficultyLevel { protected set => difficultyLevel = value;  get => difficultyLevel; }
        public static int IncreaseDiffPerScore { get => INCREASE_DIFF_PER_SCORE; }
        protected static double SpawnInterval { get => SPAWN_INTERVAL; }
        protected static double CleanInterval { get => CLEAN_INTERVAL; }


        /*       private SpeedHandler defaultEntitySpeed = new SpeedHandler(250.0, 15.0, 0);
               private SpeedHandler backgroundSpeed = new SpeedHandler(150.0, 10.0, 0);
               private Map<EntityType, SpeedHandler> entitiesSpeed =
                       Map.of(EntityType.MISSILE, new SpeedHandler(500.0, 10.0, 5000.0));*/

        protected static void UpdateTimer() => ALogics.frameTime++;

        /*
        protected SpeedHandler getEntityMovementInfo(final EntityType type)
        {
            if (this.entitiesSpeed.containsKey(type))
            {
                return this.entitiesSpeed.get(type);
            }
            return this.defaultEntitySpeed.copy();
        }

        protected SpeedHandler getBackgroundMovementInfo()
        {
            return this.backgroundSpeed.copy();
        }*/

        public abstract Cleaner GetEntitiesCleaner();
        public abstract void UpdateAll();
    }
}
