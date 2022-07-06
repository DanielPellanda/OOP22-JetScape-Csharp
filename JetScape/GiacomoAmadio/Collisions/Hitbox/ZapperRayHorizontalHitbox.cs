using System.Drawing;

namespace JetScape.Collisions.Hitbox
{
    public class ZapperRayHorizontalHitbox : HitboxInstance
    {
        private const double RECTANGLE_X = 0;
        private const double RECTANGLE_Y = 6;
        private const double RECTANGLE_W = 32;
        private const double RECTANGLE_H = 20;
        public ZapperRayHorizontalHitbox(Point startingPos) : base(startingPos)
        {
            addRectangle(RECTANGLE_X, RECTANGLE_Y, RECTANGLE_W, RECTANGLE_H);
        }
    }
}