using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetScape.game.utility
{
    public class EntityType
    {
        public const int UNDEFINED = 0;
        public const int PLAYER = 1;
        public const int MISSILE = 2;
        public const int ZAPPER = 3;
        public const int ZAPPERBASE = 4;
        public const int ZAPPERRAY = 5;
        public const int SHIELD = 6;
        public const int TELEPORT = 7;
        public const int COIN = 8;

        private enum Type { UNDEFINED = EntityType.UNDEFINED, PLAYER = EntityType.PLAYER, MISSILE = EntityType.MISSILE, 
            ZAPPER = EntityType.ZAPPER, ZAPPERBASE = EntityType.ZAPPERBASE, ZAPPERRAY = EntityType.ZAPPERRAY, 
            SHIELD = EntityType.SHIELD, TELEPORT = EntityType.TELEPORT, COIN = EntityType.COIN};
        private readonly Type _value;

        public static IList<EntityType> ALL_ENTITY_TYPE = new List<EntityType>() {
            PLAYER, ZAPPER, MISSILE, SHIELD, TELEPORT, COIN };

        private EntityType(int value) => this._value = (Type) value;

        public static implicit operator EntityType(int value) => value >= 0 && value <= 8 ? new EntityType(value) : new EntityType(0);

        public bool IsGenerableEntity()
        {
            switch (_value)
            {
                case Type.MISSILE:
                case Type.ZAPPER:
                case Type.ZAPPERBASE:
                case Type.ZAPPERRAY:
                case Type.SHIELD:
                case Type.TELEPORT:
                case Type.COIN:
                    return true;
                default:
                    break;
            }
            return false;
        }
        public bool IsObstacle()
        {
            switch (_value)
            {
                case Type.MISSILE:
                case Type.ZAPPER:
                case Type.ZAPPERBASE:
                case Type.ZAPPERRAY:
                    return true;
                default:
                    break;
            }
            return false;
        }
        public bool IsPickUp()
        {
            switch (_value)
            {
                case Type.SHIELD:
                case Type.TELEPORT:
                case Type.COIN:
                    return true;
                default:
                    break;
            }
            return false;
        }

        public static implicit operator int(EntityType type) => (int)type._value;
        public static bool operator ==(EntityType left, EntityType right) => Equals(left, right);
        public static bool operator !=(EntityType left, EntityType right) => !Equals(left, right);

        public override int GetHashCode() => _value.GetHashCode();
        public bool Equals(EntityType other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;

            return Equals(_value, other._value);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return Equals(obj as EntityType);
        }
        public override string ToString() => _value.ToString().ToLower();
    }
}
