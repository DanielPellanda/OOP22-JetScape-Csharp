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
            _options = new List<MenuOptions>
            {
                new MenuOptions(MenuOptions.MENU),
                new MenuOptions(MenuOptions.START),
                new MenuOptions(MenuOptions.SETTINGS),
                new MenuOptions(MenuOptions.RECORDS),
                new MenuOptions(MenuOptions.RETRY)
            };
            _menuHandler = new MenuHandler(_options);
        }

        [Test]
        public void Test()
        {
            for (int i = 0; i < _options.Count + 1 ; i++)
            {
                _menuHandler.GoDown();
            }
            Assert.That(
                _menuHandler.GetSelectedOption().GetOptionsGS(),
                Is.EqualTo(GameState.INGAME));
            for (int i = 0; i < _options.Count + 1; i++)
            {
                _menuHandler.GoUp();
            }
            Assert.That(
                _menuHandler.GetSelectedOption().GetOptionsGS(),
                Is.EqualTo(GameState.MENU));
        }
    }
}
