using NUnit.Framework;
using System.Collections.Generic;
using JetScape.game.utility;

namespace JetScape.Menu
{
    [TestFixture]
    public class TestMenus
    {
        private MenuHandler _menuHandler;
        private List<MenuOptions> _options;

        [SetUp]
        public void Setup()
        {
            _options = new List<MenuOptions>();
            _options.Add(new MenuOptions(MenuOptions.MENU));
            _options.Add(new MenuOptions(MenuOptions.START)); 
            _options.Add(new MenuOptions(MenuOptions.SETTINGS));
            _options.Add(new MenuOptions(MenuOptions.RECORDS));
            _options.Add(new MenuOptions(MenuOptions.RETRY));
            _menuHandler = new MenuHandler(_options);
        }

        [Test]
        public void Test()
        {
            for (int i = 0; i < _options.Count + 1 ; i++)
            {
                _menuHandler.GoDown();
            }
            Assert.That(_menuHandler.GetSelectedOption().getOptionsGS(), Is.EqualTo(GameState.INGAME));
            for (int i = 0; i < _options.Count + 1; i++)
            {
                _menuHandler.GoUp();
            }
            Assert.That(_menuHandler.GetSelectedOption().getOptionsGS(), Is.EqualTo(GameState.MENU));
        }
    }
}
