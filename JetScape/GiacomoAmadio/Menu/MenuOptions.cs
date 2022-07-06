using System.Collections.Generic;
using JetScape.game.utility;

namespace JetScape.Menu
{
    public class MenuOptions
    {
        public const int START = 0;
        public const int SETTINGS = 1;
        public const int QUIT = 2;
        public const int RECORDS = 3;
        public const int MENU = 4;
        public const int RETRY = 5;
        public const int RESUME = 6;
        public const int MUSIC = 7;
        public const int SOUND = 8;

        private enum _MenuOption { START, SETTINGS, QUIT, RECORDS,
            MENU, RETRY, RESUME, MUSIC, SOUND }
        
        private Dictionary<_MenuOption, GameState> _Options =
            new Dictionary<_MenuOption, GameState>() {
                {_MenuOption.START, GameState.INGAME},
                {_MenuOption.SETTINGS, GameState.SETTINGS},
                {_MenuOption.QUIT, GameState.EXIT},
                {_MenuOption.RECORDS, GameState.RECORDS},
                {_MenuOption.MENU, GameState.MENU},
                {_MenuOption.RETRY, GameState.INGAME},
                {_MenuOption.RESUME, GameState.INGAME},
                {_MenuOption.MUSIC, GameState.SETTINGS},
                {_MenuOption.SOUND, GameState.SETTINGS},
            };
        readonly _MenuOption _current;

        private MenuOptions(int val) => _current = (_MenuOption)val;

        public static implicit operator MenuOptions(int value)
        {
            if (value >= 0 && value <= 8) 
            {
                return new MenuOptions(value);
            }  
            return new MenuOptions(MENU);
        }

        public GameState getOptionsGS() => _Options[_current];

        public string toString() => _current.ToString().ToLower();
    }
}
