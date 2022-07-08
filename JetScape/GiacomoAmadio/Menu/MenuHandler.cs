using JetScape.game.utility;
using System.Collections.Generic;

namespace JetScape.Menu
{
    // partial implementatation , the classes for input managing  and drawing have not been translated

    public delegate void SetGameState(GameState gs);

    public class MenuHandler : IDisplayHandler
    {
        private readonly List<MenuOptions> _options;

        private readonly SetGameState _gameState;

        private int _cursor;

        private MenuOptions _selectedOption;

        public MenuHandler(List<MenuOptions> options)
        {
            _cursor = 0;
            _options = options;
            UpdateSelectedOption();
        }
        public MenuHandler(List<MenuOptions> options, SetGameState setter) : this(options)
        {
            _gameState = setter;
        }

        public MenuOptions GetSelectedOption()
        {
            UpdateSelectedOption();
            return _selectedOption;
        }

        public void Select() => _gameState(GetSelectedOption().getOptionsGS());

        public void GoUp()
        {
            _cursor--;
            if (_cursor < 0)
            {
                _cursor = _options.Count - 1;
            }
        }

        public void GoDown()
        {
            _cursor++;
            if (_cursor > _options.Count - 1)
            {
                _cursor = 0;
            }
        }

        private void UpdateSelectedOption() => _selectedOption = _options[_cursor];
    }
}
