using JetScape.game.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using JetScape.Collisions.Hitbox;

namespace JetScape.game.logics.entities
{
    public interface IEntity
    {
        bool IsVisible { get; }
        bool IsOnScreenBounds { get; }
        bool IsOnClearArea { get; }
        bool IsOnSpawnArea { get; }
        Point Position { get; }
        IHitbox EntityHitbox { get; }
        EntityType EntityType { get; }

        void Reset();
        void Clean();
        void Update();
    }
}
