using System;
using System.Collections.Generic;
using System.Drawing;
using System.Collections.Immutable;


namespace Collisions
{
    public abstract class HitboxInstance : IHitbox
    {
        private const int SPRITE_DIMENSIONS = 32;
        private const int CURRENT_TILE_SIZE = 32;
        private IDictionary<Rectangle, KeyValuePair<double, double>> _hitboxes;
        private KeyValuePair<double, double> _currentPos;
        private readonly ISet<Rectangle> _rectangles;

        public HitboxInstance(KeyValuePair<double, double> startPos)
        {
            _hitboxes = new Dictionary<Rectangle, KeyValuePair<double, double>>();
            _currentPos = startPos;
            _rectangles = new HashSet<Rectangle>();
        }

        public void UpdatePosition(KeyValuePair<double, double> pos)
        {
            var map = new Dictionary<Rectangle, KeyValuePair<double, double>>();
            foreach (Rectangle rect in _hitboxes.Keys)
            {
                map.Add(new Rectangle((int)(pos.Key + _hitboxes[rect].Key),
                    (int)(pos.Value + _hitboxes[rect].Value),
                    rect.Width,
                    rect.Height),
                    _hitboxes[rect]);
            }
            _hitboxes = map;
        }

        public ISet<Rectangle> GetRectangles() => _rectangles.ToImmutableHashSet();

        protected void addRectangle(double x, double y, double width, double height)
        {
            int startingX = (int)(_currentPos.Key + scale(x));
            int startingY = (int)(_currentPos.Value + scale(y));
            int scaledWidth = (int)scale(width);
            int scaledHeight = (int)scale(height);
            _hitboxes.Add(new Rectangle(startingX, startingY, scaledWidth, scaledHeight),
                    new KeyValuePair<double, double>(scale(x), scale(y)));
            _rectangles.UnionWith(_hitboxes.Keys);
        }

        protected void addHitbox(IHitbox hitbox) => _rectangles.UnionWith(hitbox.GetRectangles());

        private double scale(double x) => CURRENT_TILE_SIZE * (x / SPRITE_DIMENSIONS);
             
    }
}
