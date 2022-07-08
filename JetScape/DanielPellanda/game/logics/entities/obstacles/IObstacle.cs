using System;
using System.Collections.Generic;
using System.Text;

using JetScape.game.logics.interactions;

namespace JetScape.game.logics.entities.obstacles
{
    public interface IObstacle : IEntity
    {
        SpeedHandler EntityMovement { get; }
    }
}
