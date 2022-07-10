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
        private readonly IDictionary<EntityType, ISet<IEntity>> _entities;

        private readonly Player _player;

        private readonly Queue<Entity> _collisions;

        public CollisionsChecker(IDictionary<EntityType, ISet<IEntity>> entities, Player p)
        {
            _entities = entities;
            _collisions = new Queue<Entity>();
            _player = p;
        }

        public Entity NextToHandle() 
        {
            _collisions.TryDequeue(out Entity entity);
            return entity;
        }

        public void UpdateCollisions()
        {
            foreach ( KeyValuePair<EntityType, ISet<IEntity>> entry in _entities )
            {
                if (entry.Key != EntityType.PLAYER)
                {
                    foreach (Entity entity in entry.Value)
                    {
                        if (Collides(entity.EntityHitbox))
                        {
                            _collisions.Enqueue(entity);
                        }
                    }
                }
            }
        }
        private bool Collides(IHitbox entity)
        {
            foreach (Rectangle player in _player.EntityHitbox.GetRectangles())
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
