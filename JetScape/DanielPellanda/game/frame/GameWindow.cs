using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JetScape.game.frame
{
    // This class structure has been simplified for the sole porpose of testing.
    public class GameWindow
    {
        private const int SCREEN_WIDTH = 1920;
        private const int SCREEN_HEIGHT = 1080;

        public const int FPS_LIMIT = 60;
        public static GameScreen ScreenInfo { get; } = new GameScreen(SCREEN_HEIGHT, SCREEN_WIDTH);

        public class GameScreen
        {
            private const int HORIZONTAL_RATIO = 16;
            private const int VERTICAL_RATIO = 9;
            private const double PROPORTION = 1.5;

            public Tuple<int, int> CurrentSize { get; private set; }
            public int Height { get => CurrentSize.Item1; }
            public int Width { get => CurrentSize.Item2; }
            public int TileSize { get; private set; } 

            public GameScreen(int height, int width)
            {
                CurrentSize = new Tuple<int, int>(height, width);
                TileSize = (int) (Width / PROPORTION) / HORIZONTAL_RATIO;
            }
        }
    }
}
