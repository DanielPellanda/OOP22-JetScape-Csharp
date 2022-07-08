using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using JetScape.game.logics.interactions;
using JetScape.game.logics.entities.player;
using JetScape.game.utility;

namespace JetScape.game.logics.entities.pickups
{
    public class Pickup : Entity, IPickup
    {
        public SpeedHandler EntityMovement { get; private set; }
        public IPlayer PlayerEntity { get; private set; }

        protected Pickup(ILogics l, Point position, EntityType pickupType, IPlayer player, SpeedHandler speed) :
            base(l, position, pickupType)
        {
            this.PlayerEntity = player;
            this.EntityMovement = speed.Copy();

            //base.setHitbox(new PickableHitbox(position));
        }

    }
}
