using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using JetScape.game.frame;
using JetScape.game.logics.interactions;
using JetScape.game.logics.entities.player;
using JetScape.game.utility;
using JetScape.Collisions.Hitbox;

namespace JetScape.game.logics.entities.obstacles.missile
{
    public class Missile : Obstacle, IObstacle
    {
        private enum Direction { UP, DOWN };

        private int _frameTime;

        private readonly Point playerPosition;

        private const double Y_DEFAULT_SPEED = 0;
        private const double Y_DEFAULT_ACCELERATION = 400;

        private readonly double _yStartSpeed = Y_DEFAULT_SPEED;
        private double _ySpeed;
        private double _yAcceleration = Y_DEFAULT_ACCELERATION;

        private const double Y_BRAKING_DIVIDER = 3.5;
        private const double Y_BRAKE_DECREASE = 1.0;

        private Direction lastDir = Direction.UP;

        public Missile(ILogics l, Point pos, IPlayer player, SpeedHandler speed) : base(l, pos, EntityType.MISSILE, speed)
        {
            _ySpeed = _yStartSpeed;
            playerPosition = player.Position;

            EntityHitbox = new MissileHitbox(pos);
        }

        private void UpdateFrameTime()
        {
            _frameTime++;
            if (_frameTime >= GameWindow.FPS_LIMIT)
            {
                _frameTime = 0;
            }
        }


        public override void Reset()
        {
            base.Reset();
            _ySpeed = _yStartSpeed;
        }

        public override void Update()
        {
            base.Update();
            UpdateFrameTime();

            if (this.IsOnSpawnArea)
            {
                if (Position.Y > playerPosition.Y)
                {
                    if (lastDir != Direction.UP)
                    {
                        _ySpeed = -_ySpeed / Y_BRAKING_DIVIDER + Y_BRAKE_DECREASE * ALogics.DifficultyLevel;
                    }
                    SetNewPosition(Position.X, Position.Y - (int) (_ySpeed / GameWindow.FPS_LIMIT));
                    _ySpeed += _yAcceleration / GameWindow.FPS_LIMIT;
                    lastDir = Direction.UP;
                }
                else if (Position.Y < playerPosition.Y)
                {
                    if (lastDir != Direction.DOWN)
                    {
                        _ySpeed = -_ySpeed / Y_BRAKING_DIVIDER + Y_BRAKE_DECREASE * ALogics.DifficultyLevel;
                    }
                    SetNewPosition(Position.X, Position.Y + (int) (_ySpeed / GameWindow.FPS_LIMIT));
                    _ySpeed += _yAcceleration / GameWindow.FPS_LIMIT;
                    lastDir = Direction.DOWN;
                }
            }
        }
    }
}
