using System;
using System.Collections.Generic;
using System.Text;

using JetScape.game.logics.interactions;
using JetScape.game.logics.entities.player;

namespace JetScape.game.logics.entities.pickups
{
    public interface IPickup : IEntity
    {
        SpeedHandler EntityMovement { get; }
        IPlayer PlayerEntity { get; }
    }
}
