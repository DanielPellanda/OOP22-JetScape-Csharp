using System.Collections.Generic;
using System.Drawing;
using JetScape.game.utility;
using JetScape.game.logics.entities;
using JetScape.game.logics.entities.player;
using JetScape.Collisions.Hitbox;

namespace JetScape.Collisions
{
    public class CollisionsChecker
    {
        private Dictionary<EntityType, ISet<Entity>> _entities;

        private Player _player;

        private Queue<Entity> _collisions;

        public CollisionsChecker(Dictionary<EntityType, ISet<Entity>> entities, Player p)
        {
            _entities = entities;
            _collisions = new Queue<Entity>();
            _player = p;
        }

        public Entity NextToHandle() 
        {
            Entity entity = null;
            _collisions.TryDequeue(out entity);
            return entity;
        }

        public void updateCollisions()
        {
            foreach ( KeyValuePair<EntityType, ISet<Entity>> entry in _entities )
            {
                if (entry.Key != EntityType.PLAYER)
                {
                    foreach (Entity entity in entry.Value)
                    {
                        if (collides(entity.))
                        {
                            _collisions.Enqueue(entity);
                        }
                    }
                }
            }
        }
        private bool collides(IHitbox entity)
        {
            foreach (Rectangle player in _player.GetRectangles())
            {
                foreach (Rectangle target in entity.GetRectangles())
                {
                    if (player.IntersectsWith(target))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
