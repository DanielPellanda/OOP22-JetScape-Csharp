using System;
using System.Collections.Generic;
using System.Text;

using JetScape.game.logics;
using JetScape.game.frame;

namespace JetScape.game.logics.interactions
{
    public class SpeedHandler
    {
        private readonly double _startSpeed;
        private readonly double _speedIncDifficulty;
        private readonly double _acceleration;
        public double Speed { get => Speed + _speedIncDifficulty * ALogics.DifficultyLevel; private set => Speed = value; }

        public SpeedHandler(double startSpeed, double speedIncDifficulty, double acceleration)
        {
            this._startSpeed = startSpeed;
            this.Speed = _startSpeed;
            this._speedIncDifficulty = speedIncDifficulty;
            this._acceleration = acceleration;
        }

        public void ApplyAcceleration() => Speed += _acceleration / GameWindow.FPS_LIMIT;

        public void ResetSpeed() => Speed = _startSpeed;
  
        public SpeedHandler Copy() => new SpeedHandler(_startSpeed, _speedIncDifficulty, _acceleration);
    }
}
