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

        private enum MenuOption
        {
            START, SETTINGS, QUIT, RECORDS,
            MENU, RETRY, RESUME, MUSIC, SOUND
        }
        
        private readonly Dictionary<MenuOption, GameState> _Options =
            new Dictionary<MenuOption, GameState>() {
                {MenuOption.START, GameState.INGAME},
                {MenuOption.SETTINGS, GameState.SETTINGS},
                {MenuOption.QUIT, GameState.EXIT},
                {MenuOption.RECORDS, GameState.RECORDS},
                {MenuOption.MENU, GameState.MENU},
                {MenuOption.RETRY, GameState.INGAME},
                {MenuOption.RESUME, GameState.INGAME},
                {MenuOption.MUSIC, GameState.SETTINGS},
                {MenuOption.SOUND, GameState.SETTINGS},
            };
        private readonly MenuOption _current;

        public MenuOptions(int val) => _current = (MenuOption)val;

        public static implicit operator int(MenuOptions option) => (int)option._current;

        public static implicit operator MenuOptions(int value)
        {
            if (value >= 0 && value <= 8) 
            {
                return new MenuOptions(value);
            }  
            return new MenuOptions(MENU);
        }

        public GameState GetOptionsGS() => _Options[_current];

        public override string ToString() => _current.ToString().ToLower();
    }
}
