using System.Drawing;

namespace JetScape.Collisions.Hitbox
{
    public class PickableHitbox : HitboxInstance
    {
        private const double RECTANGLE_X = 0;
        private const double RECTANGLE_Y = 0;
        private const double RECTANGLE_W = 32;
        private const double RECTANGLE_H = 32;
        public PickableHitbox(Point startingPos) : base(startingPos)
        {
            AddRectangle(RECTANGLE_X, RECTANGLE_Y, RECTANGLE_W, RECTANGLE_H);
        }
    }
}