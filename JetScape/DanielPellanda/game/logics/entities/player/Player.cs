﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using JetScape.game.logics;
using JetScape.game.utility;
using JetScape.game.frame;

using JetScape.Collisions;
using JetScape.Collisions.Hitbox;

namespace JetScape.game.logics.entities.player
{
    public class Player : Entity, IPlayer
    {
        private const double BASE_FALL_SPEED = 50.0;
        private const double BASE_JUMP_SPEED = 20.0;
        private const double INITIAL_JUMP_MULTIPLIER = 1.0;
        private const double INITIAL_FALL_MULTIPLIER = 0.3;
        private const double JUMP_MULTIPLIER_INCREASE = 0.6;
        private const double FALL_MULTIPLIER_INCREASE = 0.15;
        private const double X_RELATIVE_POSITION = 2.11;

        private const int INVINCIBILITY_TIMER = 2;

        private static readonly int X_POSITION = (int) (GameWindow.ScreenInfo.TileSize * X_RELATIVE_POSITION);

        private bool _shieldProtected;
        private bool _invulnerable;

        private readonly double _jumpSpeed;
        private readonly double _fallSpeed;

        private double _jumpMultiplier = INITIAL_JUMP_MULTIPLIER;
        private double _fallMultiplier = INITIAL_FALL_MULTIPLIER;

        private int _frameTime;
        private int _invulnerableTimer = -1;

        private CollisionsHandler _hitChecker;

        private PlayerStatus _status;
        private bool _statusChanged;
        private PlayerStatus Status
        {
            get => _status;
            set
            {
                _statusChanged = _status != value;
                _status = value;
            }
        }
        private bool StatusChanged { get; }
        public bool SimulateInput { get; set; }
        public int CurrentScore { get; private set; }
        public int CurrentCoinsCollected { get; private set; }
        public bool HasDied { get => Status == PlayerStatus.DEAD; }
        public PlayerDeath CauseOfDeath
        {
            get => CauseOfDeath;
            private set
            {
                switch (value)
                {
                    case PlayerDeath.BURNED:
                    case PlayerDeath.ZAPPED:
                        CauseOfDeath = value;
                        break;
                    default:
                        CauseOfDeath = PlayerDeath.NONE;
                        break;
                }
            }
        }

        public Player(ILogics log) : base(log, new Point(X_POSITION, Y_LOW_LIMIT), EntityType.PLAYER)
        {
            _fallSpeed = BASE_FALL_SPEED / GameWindow.FPS_LIMIT;
            _jumpSpeed = BASE_JUMP_SPEED / GameWindow.FPS_LIMIT;

            EntityHitbox = new PlayerHitbox(Position);
            _hitChecker = new CollisionsHandler(log.Entities, this);

            this._status = PlayerStatus.WALK;
        }

        private void ObstacleHit(PlayerStatus statusAfterHit)
        {
            if (!this._invulnerable && !_status.IsInDyingAnimation())
            {
                if (this._shieldProtected)
                {
                    this._invulnerable = true;
                    this._shieldProtected = false;
                    return;
                }
                this.Status = statusAfterHit;
            }
        }

        private void CheckHit(IEntity entityHit)
        {
            switch (entityHit.EntityType)
            {
                case EntityType.MISSILE:
                    this.ObstacleHit(PlayerStatus.BURNED);
                    entityHit.Clean();
                    break;
                case EntityType.SHIELD:
                    this._shieldProtected = true;
                    entityHit.Clean();
                    break;
                default:
                    break;
            }
        }

        private void Jump()
        {
            this._fallMultiplier = INITIAL_FALL_MULTIPLIER;

            SetNewPosition(Position.X, Position.Y - _jumpSpeed * _jumpMultiplier > Y_TOP_LIMIT ?
                    Position.Y - (int) (_jumpSpeed * _jumpMultiplier)
                    : Y_TOP_LIMIT);
            this.Status = PlayerStatus.JUMP;
        }

        private bool Fall()
        {
            this._jumpMultiplier = INITIAL_JUMP_MULTIPLIER;

            if (Position.Y + _fallSpeed * _fallMultiplier
                    < Y_LOW_LIMIT)
            {
                SetNewPosition(Position.X, Position.Y + (int) (_fallSpeed * _fallMultiplier));
                return true;
            }
            SetNewPosition(Position.X, Y_LOW_LIMIT);
            return false;
        }

        private void ControlPlayer()
        {
            if (SimulateInput)
            {
                this.Jump();
                this._jumpMultiplier += JUMP_MULTIPLIER_INCREASE;
            }
            else if (this._status != PlayerStatus.WALK)
            {
                this.Status = this.Fall() ? PlayerStatus.FALL : PlayerStatus.LAND;
                this._fallMultiplier += FALL_MULTIPLIER_INCREASE;
            }
        }

        private void UpdateInvulnerableTimer()
        {
            if (this._invulnerable)
            {
                if (this._invulnerableTimer == -1)
                {
                    this._invulnerableTimer = ALogics.FrameTime;
                }
                else if (ALogics.FrameTime - this._invulnerableTimer
                        >= INVINCIBILITY_TIMER * GameWindow.FPS_LIMIT)
                {
                    this._invulnerable = false;
                    this._invulnerableTimer = -1;
                }
            }
        }

        private void UpdateScore()
        {
            if (this._frameTime % 2 == 0 && _frameTime != 0)
            {
                this.CurrentScore++;
            }
            _frameTime++;
        }

        public override void Reset()
        {
            base.Reset();
            this.Status = PlayerStatus.WALK;
            this.CurrentScore = 0;
            this.CurrentCoinsCollected = 0;
            this._frameTime = 0;

            this._invulnerable = false;
            this._shieldProtected = false;
        }

        public override void Update()
        {
            base.Update();
            this.UpdateInvulnerableTimer();

            if (!this.Status.IsInDyingAnimation())
            {
                this.UpdateScore();
                this.ControlPlayer();
            }

            if (this.HasDied)
            {
                this.Fall();
                this._fallMultiplier += FALL_MULTIPLIER_INCREASE * 4;
            }

            EntityHitbox.UpdatePosition(Position);
            _hitChecker.Interact(e => CheckHit(e));
        }
    }
}
