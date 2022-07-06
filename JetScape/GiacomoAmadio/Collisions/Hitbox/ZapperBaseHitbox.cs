using System.Drawing;

namespace JetScape.Collisions.Hitbox
{
    public class ZapperBaseHitbox : HitboxInstance
    {
        private const double RECTANGLE_X = 5;
        private const double RECTANGLE_Y = 5;
        private const double RECTANGLE_W = 22;
        private const double RECTANGLE_H = 22;
        public ZapperBaseHitbox(Point startingPos) : base(startingPos)
        {
            addRectangle(RECTANGLE_X, RECTANGLE_Y, RECTANGLE_W, RECTANGLE_H);
        }
    }
}
