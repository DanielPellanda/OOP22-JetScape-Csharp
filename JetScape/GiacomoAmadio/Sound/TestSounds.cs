using NUnit.Framework;
using JetScape.Menu;

namespace JetScape.Sound
{
    [TestFixture]
    public class TestSounds
    {
        private SoundManager _musicM;
        private SoundManager _soundM;
        private SettingsManager _settingsM;
        private SettingsManager _settingsS;

        [SetUp]
        public void Setup()
        {
            _musicM = new SoundManager(new MenuOptions(MenuOptions.MUSIC));
            _soundM = new SoundManager(new MenuOptions(MenuOptions.SOUND));
            _settingsS = new SettingsManager(new MenuOptions(MenuOptions.SOUND));
            _settingsM = new SettingsManager(new MenuOptions(MenuOptions.MUSIC));
        }

        [Test]
        public void Test()
        {
            _settingsS.WriteSetting(2);
            _settingsM.WriteSetting(2);
            _settingsM.WriteSetting(4);
            _settingsS.WriteSetting(0);
            Assert.Multiple(() =>
            {
                Assert.That(_settingsS.GetSettingValue(), Is.EqualTo(0));
                Assert.That(_settingsM.GetSettingValue(), Is.EqualTo(4));
            });
            for (int i = 0; i < 5; i++)
            {
                _musicM.LowerVolumeLevel();
                _soundM.RaiseVolumeLevel();
            }
            Assert.Multiple(() =>
            {
                Assert.That(_musicM.VolumeLevel, Is.EqualTo(0));
                Assert.That(_soundM.VolumeLevel, Is.EqualTo(4));
            });
        }
    }
}