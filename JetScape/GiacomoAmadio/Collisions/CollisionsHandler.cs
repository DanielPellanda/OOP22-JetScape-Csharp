using System.Collections.Generic;
using JetScape.game.utility;
using JetScape.game.logics.entities;
using JetScape.game.logics.entities.player;

namespace JetScape.Collisions
{
    public delegate void Rection(Entity e);
    public class CollisionsHandler
    {
        private  CollisionsChecker cChecker; 

    public CollisionsHandler(Dictionary<EntityType, ISet<Entity>> entities,  Player p)
        {
            this.cChecker = new CollisionsChecker(entities, p);
        }

        public void interact( Rection action )
        {
            cChecker.updateCollisions();
            var entity = cChecker.NextToHandle();
            while (entity != null)
            {
                action.Invoke(entity);
                entity = cChecker.NextToHandle();
            }
        }
    }
}
