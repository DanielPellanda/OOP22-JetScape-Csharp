using System.Drawing;

namespace JetScape.Collisions.Hitbox
{
    public class MissileHitbox : HitboxInstance
    {
        private const double RECTANGLE_X = 0;
        private const double RECTANGLE_Y = 13;
        private const double RECTANGLE_W = 30;
        private const double RECTANGLE_H = 5;
        public MissileHitbox(Point startingPos) : base(startingPos)
        {
            AddRectangle(RECTANGLE_X, RECTANGLE_Y, RECTANGLE_W, RECTANGLE_H);
        }
    }
}
