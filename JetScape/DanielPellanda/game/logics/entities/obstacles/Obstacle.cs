using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using JetScape.game.utility;
using JetScape.game.frame;

using JetScape.game.logics.interactions;

namespace JetScape.game.logics.entities.obstacles
{
    public class Obstacle : Entity, IObstacle
    {
        public SpeedHandler EntityMovement { get; private set; }

        protected Obstacle(ILogics log, Point position, EntityType obstacleType, SpeedHandler speed) : base(log, position, obstacleType)
        {
            EntityMovement = speed.Copy();
        }

        public override void Reset()
        {
            base.Reset();
            EntityMovement.ResetSpeed();
        }

        public override void Update()
        {
            base.Update();

            if (Position.X > -GameWindow.ScreenInfo.TileSize * 2)
            {
                SetNewPosition(Position.X - (int) (EntityMovement.Speed / GameWindow.FPS_LIMIT), Position.Y);
                if (!this.IsOnSpawnArea)
                {
                    EntityMovement.ApplyAcceleration();
                }
            }
            EntityHitbox.UpdatePosition(Position);
        }
    }
}
