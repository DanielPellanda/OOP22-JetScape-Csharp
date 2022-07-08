using JetScape.Sound;
using System;
using JetScape.Menu;

namespace JetScape.game.frame
{
    // This class structure has been simplified for the sole porpose of testing.
    public class GameWindow
    {
        private const int SCREEN_WIDTH = 1920;
        private const int SCREEN_HEIGHT = 1080;

        public static SoundManager GAME_MUSIC = new SoundManager(MenuOptions.MUSIC);
        public static SoundManager GAME_SOUND = new SoundManager(MenuOptions.SOUND);

        public const int FPS_LIMIT = 60;
        public static GameScreen ScreenInfo { get; } = new GameScreen(SCREEN_HEIGHT, SCREEN_WIDTH);

        public static long NanoTimeFromEpoch() => (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks * 100;

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
