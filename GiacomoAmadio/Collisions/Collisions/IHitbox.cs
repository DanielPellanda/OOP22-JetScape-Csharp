using System.Collections.Generic;
using System.Drawing;

namespace GiacomoAmadio.Collisions
{
    public interface IHitbox
    {
        void UpdatePosition(KeyValuePair<double, double> pos);

        ISet<Rectangle> GetRectangles();
    }
}
