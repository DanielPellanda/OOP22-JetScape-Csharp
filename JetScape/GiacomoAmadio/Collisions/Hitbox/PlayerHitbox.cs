using System.Drawing;

namespace JetScape.Collisions.Hitbox
{
    public class PlayerHitbox : HitboxInstance
    {
        private const double RECTANGLE_1X = 16;
        private const double RECTANGLE_1Y = 24;
        private const double RECTANGLE_1W = 8;
        private const double RECTANGLE_1H = 8;
        private const double RECTANGLE_2X = 16;
        private const double  RECTANGLE_2Y = 3;
        private const double RECTANGLE_2W = 13;
        private const double RECTANGLE_2H = 21;

        public PlayerHitbox( Point startingPos) : base (startingPos)
        {
            addRectangle(RECTANGLE_1X, RECTANGLE_1Y, RECTANGLE_1W, RECTANGLE_1H);
            addRectangle(RECTANGLE_2X, RECTANGLE_2Y, RECTANGLE_2W, RECTANGLE_2H);
        }
    }
}