using System.Drawing;

namespace JetScape.Collisions.Hitbox
{
    public class CoinHitbox : HitboxInstance
    {
        private const double RECTANGLE_X = 8;
        private const double RECTANGLE_Y = 8;
        private const double RECTANGLE_W = 16;
        private const double RECTANGLE_H = 16;
        public CoinHitbox(Point startingPos) : base(startingPos)
        {
            addRectangle(RECTANGLE_X, RECTANGLE_Y, RECTANGLE_W, RECTANGLE_H);
        }
    }
}
