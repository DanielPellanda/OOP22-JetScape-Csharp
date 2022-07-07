using JetScape.game.frame;
using System.Collections.Generic;

namespace JetScape.Menu
{
    public class SettingsHandler : MenuHandler
    {
        public SettingsHandler(SetGameState gs, List<MenuOptions> list) : base(list, gs) 
        {
        }

        public void Lower()
        {
            switch (GetSelectedOption())
            {
                case MenuOptions.MUSIC:
                    GameWindow.GAME_MUSIC.LowerVolumeLevel();
                    break;
                case MenuOptions.SOUND:
                    GameWindow.GAME_SOUND.LowerVolumeLevel();
                    break;
                default:
                    break;
            }
        }

        public void Raise()
        {
            switch (GetSelectedOption())
            {
                case MenuOptions.MUSIC:
                    GameWindow.GAME_MUSIC.RaiseVolumeLevel();
                    break;
                case MenuOptions.SOUND:
                    GameWindow.GAME_SOUND.RaiseVolumeLevel();
                    break;
                default:
                    break;
            }
        }
    }
}
