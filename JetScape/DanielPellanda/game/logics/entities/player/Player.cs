using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using JetScape.game.logics;
using JetScape.game.utility;
using JetScape.game.frame;

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

        private const int FLICKERING_SPEED = 10;
        private const int INVINCIBILITY_TIMER = 2;

        private const double ANIMATION_SPEED = 7;

        private const int X_POSITION = GameWindow.GAME_SCREEN.getTileSize() * X_RELATIVE_POSITION;

        private bool _shieldProtected;
        private bool _invulnerable;

 
        private int _score;
        private int _coins;

        private readonly double _jumpSpeed;
        private readonly double _fallSpeed;

        private double _jumpMultiplier = INITIAL_JUMP_MULTIPLIER;
        private double _fallMultiplier = INITIAL_FALL_MULTIPLIER;

        private int _frameTime;
        private int _invulnerableTimer = -1;

        //private final CollisionsHandler hitChecker;

        private PlayerStatus _status;
        private bool _statusChanged;

        public PlayerDeath CauseOfDeath { get; private set; }

        public Player(ILogics log) : base(log, new Point(X_POSITION, Y_LOW_LIMIT), EntityType.PLAYER)
        { 
            this._fallSpeed = BASE_FALL_SPEED / GameWindow.FPS_LIMIT;
            this._jumpSpeed = BASE_JUMP_SPEED / GameWindow.FPS_LIMIT;

            //this.setHitbox(new PlayerHitbox(this.getPosition()));
            //this.hitChecker = new CollisionsHandler(l.getEntities(), this);

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
                    //GameWindow.GAME_SOUND.play(Sound.SHIELD_DOWN);
                    return;
                }
                this.SetStatus(statusAfterHit);
                //this.SetCauseOfDeath(statusAfterHit);
            }
        }

        private void checkHit(IEntity entityHit)
        {
            switch (entityHit.EntityType)
            {
                case MISSILE:
                    if (!this._shieldProtected)
                    {
                        GameWindow.GAME_SOUND.stop(Sound.JETPACK);
                        GameWindow.GAME_SOUND.play(Sound.MISSILE);
                    }
                    this.ObstacleHit(PlayerStatus.BURNED);
                    entityHit.clean();
                    break;
                case ZAPPER:
                    if (!this._shieldProtected)
                    {
                        GameWindow.GAME_SOUND.stop(Sound.JETPACK);
                        GameWindow.GAME_SOUND.play(Sound.ZAPPED);
                    }
                    this.ObstacleHit(PlayerStatus.ZAPPED);
                    break;
                case SHIELD:
                    this._shieldProtected = true;
                    entityHit.clean();
                    GameWindow.GAME_SOUND.play(Sound.SHIELD_UP);
                    break;
                case TELEPORT:
                    this._score += TeleportInstance.getScoreIncrease();
                    this.getCleaner().accept(t->t.isGenerableEntity(), e-> true);
                    GameWindow.GAME_SOUND.play(Sound.TELEPORT);
                    break;
                case COIN:
                    this._coins++;
                    entityHit.clean();
                    GameWindow.GAME_SOUND.play(Sound.COIN);
                    break;
                default:
                    break;
            }
        }

        private void setStatus(final PlayerStatus newStatus)
        {
            this._statusChanged = this._status != newStatus;
            this._status = newStatus;
        }

        private void jump()
        {
            this._fallMultiplier = INITIAL_FALL_MULTIPLIER;

            this.getPosition().setY(this.getPosition().getY() - this._jumpSpeed
                    * this._jumpMultiplier > Y_TOP_LIMIT
                    ? this.getPosition().getY() - this._jumpSpeed * this._jumpMultiplier
                    : Y_TOP_LIMIT);
            this.setStatus(PlayerStatus.JUMP);
            if (_statusChanged)
            {
                GameWindow.GAME_SOUND.playInLoop(Sound.JETPACK);
            }
        }

        private boolean fall()
        {
            this._jumpMultiplier = INITIAL_JUMP_MULTIPLIER;

            if (this.getPosition().getY() + this._fallSpeed * this._fallMultiplier
                    < Y_LOW_LIMIT)
            {
                this.getPosition().setY(this.getPosition().getY()
                        + this._fallSpeed * this._fallMultiplier);
                return true;
            }
            this.getPosition().setY(Y_LOW_LIMIT);
            return false;
        }

        private void controlPlayer()
        {
            if (keyH.getCurrentInput(KeyEvent.VK_SPACE))
            {
                this.jump();
                this._jumpMultiplier += JUMP_MULTIPLIER_INCREASE;
            }
            else if (this._status != PlayerStatus.WALK)
            {
                this.setStatus(this.fall() ? PlayerStatus.FALL : PlayerStatus.LAND);
                this._fallMultiplier += FALL_MULTIPLIER_INCREASE;
                if (this._statusChanged)
                {
                    GameWindow.GAME_SOUND.stop(Sound.JETPACK);
                }
            }
        }

        /**
         * Updates the sprite that should be display during the animation.
         */
        private void updateSprite()
        {
            final int lastDeathSprite = 7;
            final int lastLandSprite = 3;

            if (this._statusChanged)
            {
                this._frameTime = 0;
                this.spriteSwitcher = 0;
                this._statusChanged = false;
            }
            else if (this._frameTime >= GameWindow.FPS_LIMIT / ANIMATION_SPEED)
            {
                if (this._status.isInDyingAnimation()
                        && this.spriteSwitcher >= lastDeathSprite)
                {
                    this.setStatus(PlayerStatus.DEAD);
                }
                if (this._status == PlayerStatus.LAND
                        && this.spriteSwitcher >= lastLandSprite)
                {
                    this.setStatus(PlayerStatus.WALK);
                }
                this._frameTime = 0;
                this.spriteSwitcher++;
            }
            this._frameTime++;
        }

        private void updateInvulnerableTimer()
        {
            if (this._invulnerable)
            {
                if (this._invulnerableTimer == -1)
                {
                    this._invulnerableTimer = AbstractLogics.getFrameTime();
                }
                else if (AbstractLogics.getFrameTime() - this._invulnerableTimer
                        >= INVINCIBILITY_TIMER * GameWindow.FPS_LIMIT)
                {
                    this._invulnerable = false;
                    this._invulnerableTimer = -1;
                }
            }
        }

        private void updateScore()
        {
            if (this._frameTime % 2 == 0)
            {
                this._score++;
            }
        }

        /**
         * {@inheritDoc}
         */
        public int getCurrentScore()
        {
            return this._score;
        }

        /**
         * {@inheritDoc}
         */
        public int getCurrentCoinsCollected()
        {
            return this._coins;
        }

        /**
         * {@inheritDoc}
         */
        public boolean hasDied()
        {
            return this._status == PlayerStatus.DEAD;
        }

        private void setCauseOfDeath(final PlayerStatus deathCause)
        {
            switch (deathCause)
            {
                case BURNED:
                    this.causeOfDeath = Player.PlayerDeath.BURNED;
                    break;
                case ZAPPED:
                    this.causeOfDeath = Player.PlayerDeath.ZAPPED;
                    break;
                default:
                    this.causeOfDeath = Player.PlayerDeath.NONE;
                    break;
            }
        }

        /**
         * {@inheritDoc}
         */
        public Player.PlayerDeath getCauseOfDeath()
        {
            return this.causeOfDeath;
        }

        /**
         * {@inheritDoc}
         */
        @Override
    public void reset()
        {
            super.reset();
            this.setStatus(PlayerStatus.WALK);
            this._score = 0;
            this._coins = 0;
            this._frameTime = 0;

            this._invulnerable = false;
            this._shieldProtected = false;
        }

        /**
         * {@inheritDoc}
         */
        @Override
    public void update()
        {
            super.update();
            this.updateSprite();
            this.updateInvulnerableTimer();

            if (!this._status.isInDyingAnimation())
            {
                this.updateScore();
                this.controlPlayer();
            }

            if (this.hasDied())
            {
                this.fall();
                this._fallMultiplier += FALL_MULTIPLIER_INCREASE * 4;
            }

            this.shieldPosition.setX(this.getPosition().getX() + GameWindow.GAME_SCREEN.getTileSize() / 16.0);
            this.shieldPosition.setY(this.getPosition().getY());

            this.getHitbox().updatePosition(this.getPosition());
            this.hitChecker.interact(e->checkHit(e));
        }

    }
}
