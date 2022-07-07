using System.Collections.Generic;

namespace JetScape.Sound
{
    public class Sound
    {
        public const int MAIN_THEME = 0;
        public const int MENU_SELECTION = 1;
        public const int ZAPPED = 2;
        public const int COIN = 3;
        public const int SHIELD_UP = 4;
        public const int SHIELD_DOWN = 5;
        public const int TELEPORT = 6;
        public const int MISSILE = 7;
        public const int JETPACK = 8;
        public const int MISSILE_WARNING = 9;
        public const int NONE = 10;

        private enum _Sound
        {
            MAIN_THEME, MENU_SELECTION, ZAPPED, COIN, SHIELD_UP,
            SHIELD_DOWN, TELEPORT, MISSILE, JETPACK, MISSILE_WARNING, NONE
        }

        private Dictionary<_Sound, string> _Sounds =
            new Dictionary<_Sound, string>() {
                {_Sound.MAIN_THEME, "MainTheme.wav"},
                {_Sound.MENU_SELECTION, "MenuSelect.wav"},
                {_Sound.ZAPPED, "Zapped.wav"},
                {_Sound.COIN, "Coin.wav"},
                {_Sound.SHIELD_UP, "ShieldUp.wav"},
                {_Sound.SHIELD_DOWN, "ShieldDown.wav"},
                {_Sound.TELEPORT, "Teleport.wav"},
                {_Sound.MISSILE, "Missile.wav"},
                {_Sound.JETPACK, "Jetpack.wav"},
                {_Sound.MISSILE_WARNING, "MissileWarning.wav"}
            };

        readonly _Sound _current;

        private Sound(int val) => _current = (_Sound)val;

        public static implicit operator Sound(int value)
        {
            if (value >= 0 && value <= 9)
            {
                return new Sound(value);
            }
            return new Sound(NONE);
        }

        public static implicit operator int(Sound snd) => (int)snd._current;
        public string GetFileName() => _Sounds[_current];
    }
}
