using System.Collections.Generic;
using System.Drawing;

namespace GiacomoAmadio.Collisions
{
    public interface IHitbox
    {
        void UpdatePosition(Point pos);

        ISet<Rectangle> GetRectangles();
    }
}
