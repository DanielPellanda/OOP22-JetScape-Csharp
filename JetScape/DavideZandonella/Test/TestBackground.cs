using NUnit.Framework;

using JetScape.Background.IBackground;
using JetScape.Background.BackgroundController;

namespace Jetscape.Test
{
    [TestFixture]
    public class TestBackground
    {
        private SpeedHandler _speedhandler;
        private IBackground _background;

        [SetUp]
        public void Setup()
        {
            _speedhandler = new Speedhandler(150, 10, 0);
            _background = new BackgroundController(_speedhandler);
        }

        [Test]
        public void Test()
        {
            Assert.True(_background.IsVisible());

           _background.Update();

            Assert.True(_background.IsVisible());

			int i = 0;

            // Wait all the background sprite shifted to the left
			while (i <= GameWindow.GameScreen.GetWidth())
            {
               i = i + _movement.GetXSpeed() / GameWindow.FpsLimit;
               _background.Update();
            }

            Assert.True(_background.IsVisible());
        }
    }
}
