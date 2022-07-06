using System.Collections.Generic;
using System.Drawing;

namespace JetScape.Collisions.Hitbox
{
    public interface IHitbox
    {
        void UpdatePosition(Point pos);

        ISet<Rectangle> GetRectangles();
    }
}
