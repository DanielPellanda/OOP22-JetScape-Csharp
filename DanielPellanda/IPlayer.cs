using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetScape.DanielPellanda
{
    public interface IPlayer : IEntity
    {
        bool hasDied();
        int CurrentScore();
        int CurrentCoinsCollected();

        PlayerDeath CauseOfDeath();

        enum PlayerStatus { WALK, LAND, FALL, JUMP, ZAPPED, BURNED, DEAD };

        /*
        public boolean isInDyingAnimation()
        {
            switch (this)
            {
                case ZAPPED:
                case BURNED:
                case DEAD:
                    return true;
                default:
                    break;
            }
            return false;
        }

        /*public boolean isChanged() {
            return PlayerStatus.hasChanged;
        }*


        public String toString()
        {
            return super.toString().toLowerCase(Locale.ENGLISH);
        }
    }*/
    enum PlayerDeath { BURNED, ZAPPED, NONE }
}
}
