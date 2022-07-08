using System.Drawing;

namespace JetScape.Collisions.Hitbox
{
    public class ZapperRayVerticalHitbox : HitboxInstance
    {
        private const double RECTANGLE_X = 6;
        private const double RECTANGLE_Y = 0;
        private const double RECTANGLE_W = 20;
        private const double RECTANGLE_H = 32;
        public ZapperRayVerticalHitbox(Point startingPos) : base(startingPos)
        {
            AddRectangle(RECTANGLE_X, RECTANGLE_Y, RECTANGLE_W, RECTANGLE_H);
        }
    }
}