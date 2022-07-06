using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JetScape.game.logics
{
    public abstract class ALogics : ILogics
    { 
        public static int FrameTime { get; private set; }
        public static int DifficultyLevel { protected set; get; } = 1;
        public static int IncreaseDiffPerScore { get; } = 250;
        internal static double SpawnInterval { get; } = 3.3;
        internal static double CleanInterval { get; } = 5.0;


        /*       private SpeedHandler defaultEntitySpeed = new SpeedHandler(250.0, 15.0, 0);
               private SpeedHandler backgroundSpeed = new SpeedHandler(150.0, 10.0, 0);
               private Map<EntityType, SpeedHandler> entitiesSpeed =
                       Map.of(EntityType.MISSILE, new SpeedHandler(500.0, 10.0, 5000.0));*/

        internal static void UpdateTimer() => FrameTime++;

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
