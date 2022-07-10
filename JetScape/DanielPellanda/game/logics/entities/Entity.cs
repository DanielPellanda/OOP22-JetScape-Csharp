using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using JetScape.game.utility;
using JetScape.game.frame;

using JetScape.Collisions.Hitbox;

namespace JetScape.game.logics.entities
{
    public class Entity : IEntity
    {
        protected static readonly int Y_TOP_LIMIT = 0;
        protected static readonly int Y_LOW_LIMIT = GameWindow.ScreenInfo.Height - (GameWindow.ScreenInfo.TileSize * 2);

        private readonly Point _startPos;
        private Point _position;

        private readonly EntityType _entityTag;

        private bool _visible = true;
        private bool _onScreen;
        private bool _onClearArea;
        private bool _onSpawnArea = true;

        private readonly Cleaner _cleaner;

        public Point Position { get => _position; }
        public EntityType EntityType { get => _entityTag; }
        public IHitbox EntityHitbox { get; protected set; }
        protected Cleaner EntityCleaner { get => _cleaner; }

        public bool IsVisible { get => _visible; protected set => _visible = value; }
        public bool IsOnScreenBounds { get => _onScreen; private set => _onScreen = value; }
        public bool IsOnClearArea { get => _onClearArea; private set => _onClearArea = value; }
        public bool IsOnSpawnArea { get => _onSpawnArea; private set => _onSpawnArea = value; }

        protected Entity(ILogics l, Point position, int type)
        {
            this._cleaner = l.GetEntitiesCleaner();
            this._position = position;
            this._startPos = new Point(Position.X, Position.Y);
            this._entityTag = type;

            this.IsVisible = true;
        }

        protected void SetNewPosition(int x, int y)
        {
            this._position.X = x;
            this._position.Y = y;
        }

        public virtual void Reset()
        {
            SetNewPosition(_startPos.X, _startPos.Y);
            EntityHitbox.UpdatePosition(_position);
        }

        public virtual void Clean()
        {
            Reset();
            EntityCleaner.Invoke(t => EntityType == t, e => this == e);
        }

        private void UpdateFlags()
        {
            if (Position.X >= -GameWindow.ScreenInfo.TileSize
                    && Position.X <= GameWindow.ScreenInfo.Width
                    && Position.Y >= 0 && Position.Y <= GameWindow.ScreenInfo.Height)
            {
                _onScreen = true;
                _onClearArea = false;
                _onSpawnArea = false;
            }
            else
            {
                if (Position.X < -GameWindow.ScreenInfo.TileSize)
                {
                    _onClearArea = true;
                    _onSpawnArea = false;
                }
                else if (Position.X >= GameWindow.ScreenInfo.Width)
                {
                    _onClearArea = false;
                    _onSpawnArea = true;
                }
                else
                {
                    _onClearArea = false;
                    _onSpawnArea = false;
                }
                _onScreen = false;
            }
        }

        public virtual void Update() => this.UpdateFlags();

        public override String ToString() => EntityType.ToString() + "[X:" + Position.X + "-Y:" + Position.Y + "]";
    }
}
