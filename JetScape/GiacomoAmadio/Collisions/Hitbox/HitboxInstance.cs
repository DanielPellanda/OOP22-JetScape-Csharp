using System.Collections.Generic;
using System.Drawing;
using System.Collections.Immutable;


namespace JetScape.Collisions.Hitbox
{
    public abstract class HitboxInstance : IHitbox
    {
        private const int SPRITE_DIMENSIONS = 32;
        private const int CURRENT_TILE_SIZE = 32;
        private IDictionary<Rectangle, Point> _hitboxes;
        private Point _currentPos;
        private readonly ISet<Rectangle> _rectangles;

        public HitboxInstance(Point startPos)
        {
            _hitboxes = new Dictionary<Rectangle, Point>();
            _currentPos = startPos;
            _rectangles = new HashSet<Rectangle>();
        }

        public void UpdatePosition(Point pos)
        {
            var map = new Dictionary<Rectangle, Point>();
            foreach (Rectangle rect in _hitboxes.Keys)
            {
                map.Add(new Rectangle(pos.X + _hitboxes[rect].X, pos.Y + _hitboxes[rect].Y,
                    rect.Width, rect.Height), _hitboxes[rect]);
            }
            _hitboxes = map;
        }

        public ISet<Rectangle> GetRectangles() => _rectangles.ToImmutableHashSet();

        protected void AddRectangle(double x, double y, double width, double height)
        {
            int startingX = (int)(_currentPos.X + Scale(x));
            int startingY = (int)(_currentPos.Y + Scale(y));
            int scaledWidth = (int)Scale(width);
            int scaledHeight = (int)Scale(height);
            _hitboxes.Add(new Rectangle(startingX, startingY, scaledWidth, scaledHeight),
                    new Point((int)Scale(x), (int)Scale(y)));
            _rectangles.UnionWith(_hitboxes.Keys);
        }

        protected void AddHitbox(IHitbox hitbox) => _rectangles.UnionWith(hitbox.GetRectangles());

        private double Scale(double x) => CURRENT_TILE_SIZE * (x / SPRITE_DIMENSIONS);

    }
}
