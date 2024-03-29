﻿using System.Collections.Generic;
using JetScape.game.utility;
using JetScape.game.logics.entities;
using JetScape.game.logics.entities.player;

namespace JetScape.Collisions
{
    public delegate void Reaction(Entity e);
    public class CollisionsHandler
    {
        private readonly CollisionsChecker _cChecker;

        public CollisionsHandler(IDictionary<EntityType, ISet<IEntity>> entities, Player p)
        {
            _cChecker = new CollisionsChecker(entities, p);
        }

        public void Interact( Reaction action )
        {
            _cChecker.UpdateCollisions();
            var entity = _cChecker.NextToHandle();
            while (entity != null)
            {
                action.Invoke(entity);
                entity = _cChecker.NextToHandle();
            }
        }
    }
}
