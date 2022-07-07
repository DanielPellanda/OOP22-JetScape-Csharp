using JetScape.Menu;

namespace JetScape.Sound
{
    // partial implementation, .net core doesn't support audio playing
    public class SoundManager
    {
        private const int MAX_LEVEL = 4;
        private const int MIN_LEVEL = 0;
        private SettingsManager _sManager;

        private int VolumeLevel { get; set; }

        public SoundManager( MenuOptions audioSetting)
        {
            _sManager = new SettingsManager(audioSetting);
            VolumeLevel = _sManager.GetSettingValue();
        }

        public void LowerVolumeLevel()
        {
            if (VolumeLevel > MIN_LEVEL)
            {
                VolumeLevel--;
                _sManager.WriteSetting(VolumeLevel);
            }
        }

        public void RaiseVolumeLevel()
        {
            if (VolumeLevel < MAX_LEVEL)
            {
                VolumeLevel++;
                _sManager.WriteSetting(VolumeLevel);
            }
        }

    }
}
