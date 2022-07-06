using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms.PaintEventHandler;

namespace Collisions
{
    public interface IHitbox
    {
        void UpdatePosition(KeyValuePair<double, double> pos);

        ISet<Rectangle> GetRectangles();
    }
}
