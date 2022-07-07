using System.IO;
using System.Collections.Generic;
using System.Text.Json;

namespace JetScape.Menu
{
    public class SettingsManager
    {
        private const int DEFAULT_SETTING = 2;

        private Dictionary<string, int> _setting;

        private const string filename = "settings.json";

        public MenuOptions _option { get; private set; }

        public SettingsManager(MenuOptions option)
        {
            _setting = new Dictionary<string, int>();
            _option = option;
        }

        public void WriteSetting(int value)
        {
            Update();
            _setting.Remove(_option.ToString());
            _setting.Add(_option.ToString(), value);
            Write();
        }

        public int GetSettingValue()
        {
            Update();
            return _setting.GetValueOrDefault(_option.ToString());
        }

        private void Write()
        {
            string str =  JsonSerializer.Serialize(_setting,
                new JsonSerializerOptions() { WriteIndented = true });
            StreamWriter wrt = new StreamWriter(filename);
            wrt.Write(str);
        }

        private void Update()
        {
            if (!File.Exists(filename)) 
            {
                BuildDefault();
                Write();
            }

            StreamReader reader = new StreamReader(filename);
            _setting = JsonSerializer.Deserialize<Dictionary<string, int>>(reader.ReadToEnd());
        }

        private void BuildDefault() 
        {
            _setting.Add(new MenuOptions(MenuOptions.MUSIC).ToString(), DEFAULT_SETTING);
            _setting.Add(new MenuOptions(MenuOptions.SOUND).ToString(), DEFAULT_SETTING);
        }
    }
}
