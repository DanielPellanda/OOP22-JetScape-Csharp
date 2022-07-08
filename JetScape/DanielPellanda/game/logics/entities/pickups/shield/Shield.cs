using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using JetScape.game.logics.entities.player;
using JetScape.game.logics.interactions;
using JetScape.game.utility;

namespace JetScape.game.logics.entities.pickups.shield
{
    public class Shield : Pickup, IPickup
    {
        public Shield(ILogics l, Point position, IPlayer player, SpeedHandler speed) : base(l, position, EntityType.SHIELD, player, speed) { }
    }
}
