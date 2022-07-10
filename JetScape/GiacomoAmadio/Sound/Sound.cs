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

        private enum SoundType
        {
            MAIN_THEME, MENU_SELECTION, ZAPPED, COIN, SHIELD_UP,
            SHIELD_DOWN, TELEPORT, MISSILE, JETPACK, MISSILE_WARNING, NONE
        }

        private readonly Dictionary<SoundType, string> _Sounds =
            new Dictionary<SoundType, string>() {
                {SoundType.MAIN_THEME, "MainTheme.wav"},
                {SoundType.MENU_SELECTION, "MenuSelect.wav"},
                {SoundType.ZAPPED, "Zapped.wav"},
                {SoundType.COIN, "Coin.wav"},
                {SoundType.SHIELD_UP, "ShieldUp.wav"},
                {SoundType.SHIELD_DOWN, "ShieldDown.wav"},
                {SoundType.TELEPORT, "Teleport.wav"},
                {SoundType.MISSILE, "Missile.wav"},
                {SoundType.JETPACK, "Jetpack.wav"},
                {SoundType.MISSILE_WARNING, "MissileWarning.wav"}
            };

        readonly SoundType _current;

        private Sound(int val) => _current = (SoundType)val;

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
