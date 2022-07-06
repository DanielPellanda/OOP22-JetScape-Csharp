using JetScape.game.logics.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetScape.game.logics.entities.player
{
    public interface IPlayer : IEntity
    {
        bool HasDied { get; }
        int CurrentScore { get; }
        int CurrentCoinsCollected { get; }
        PlayerDeath CauseOfDeath { get; }
    }

    public enum PlayerDeath { BURNED, ZAPPED, NONE }

    internal class PlayerStatus
    {
        public const int WALK = 0;
        public const int LAND = 1;
        public const int FALL = 2;
        public const int JUMP = 3;
        public const int ZAPPED = 4;
        public const int BURNED = 5;
        public const int DEAD = 6;

        private enum Status
        {
            WALK = PlayerStatus.WALK, LAND = PlayerStatus.LAND, FALL = PlayerStatus.FALL,
            JUMP = PlayerStatus.JUMP, ZAPPED = PlayerStatus.ZAPPED, BURNED = PlayerStatus.BURNED, DEAD = PlayerStatus.DEAD
        };
        private Status _value = PlayerStatus.WALK;

        private PlayerStatus(int value) => this._value = (Status)value;

        public static implicit operator PlayerStatus(int value) => value >= 0 && value <= 6 ? new PlayerStatus(value) : new PlayerStatus(0);

        public bool IsInDyingAnimation()
        {
            switch (_value)
            {
                case Status.ZAPPED:
                case Status.BURNED:
                case Status.DEAD:
                    return true;
                default:
                    break;
            }
            return false;
        }

        public override string ToString() => base.ToString().ToLower();

        public static implicit operator int(PlayerStatus status) => (int)status._value;
        public static bool operator ==(PlayerStatus left, PlayerStatus right) => Equals(left, right);
        public static bool operator !=(PlayerStatus left, PlayerStatus right) => !Equals(left, right);

        public override int GetHashCode() => _value.GetHashCode();
        public bool Equals(PlayerStatus other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;

            return Equals(_value, other._value);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return Equals(obj as PlayerStatus);
        }
    }
}
