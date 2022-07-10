using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JetScape.game.logics.interactions;
using JetScape.game.utility;

namespace JetScape.game.logics
{
    public abstract class ALogics
    {
        public static int FrameTime { get; private set; }
        public static int DifficultyLevel { protected set; get; } = 1;
        public static int IncreaseDiffPerScore { get; } = 250;
        protected static double SpawnInterval { get; } = 3.3;
        protected static double CleanInterval { get; } = 5.0;


        private SpeedHandler _defaultEntitySpeed = new SpeedHandler(250.0, 15.0, 0);
        private SpeedHandler _backgroundSpeed = new SpeedHandler(150.0, 10.0, 0);
        private IDictionary<EntityType, SpeedHandler> _entitiesSpeed = new Dictionary<EntityType, SpeedHandler>()
                       { { EntityType.MISSILE, new SpeedHandler(500.0, 10.0, 5000.0) } };

        internal static void UpdateTimer() => FrameTime++;

        protected SpeedHandler GetEntityMovementInfo(EntityType type)
        {
            if (_entitiesSpeed.ContainsKey(type))
            {
                return _entitiesSpeed[type];
            }
            return _defaultEntitySpeed.Copy();
        }

        protected SpeedHandler GetBackgroundMovementInfo()
        {
            return _backgroundSpeed.Copy();
        }

        public abstract Cleaner GetEntitiesCleaner();
        public abstract void UpdateAll();
    }
}
