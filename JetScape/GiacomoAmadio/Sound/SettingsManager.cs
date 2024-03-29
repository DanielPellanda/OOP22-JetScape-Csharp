﻿using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using JetScape.Menu;

namespace JetScape.Sound
{
    public class SettingsManager
    {
        private const int DEFAULT_SETTING = 2;

        private Dictionary<string, int> _setting;

        private const string filename = "settings.json";

        public MenuOptions Option { get; private set; }

        public SettingsManager(MenuOptions option)
        {
            _setting = new Dictionary<string, int>();
            Option = option;
        }

        public void WriteSetting(int value)
        {
            Update();
            _setting.Remove(Option.ToString());
            _setting.Add(Option.ToString(), value);
            Write();
        }

        public int GetSettingValue()
        {
            Update();
            return _setting.GetValueOrDefault(Option.ToString());
        }

        private void Write()
        {
            string str =  JsonSerializer.Serialize(_setting,
                new JsonSerializerOptions() { WriteIndented = true });
            StreamWriter wrt = new StreamWriter(filename);
            wrt.Write(str);
            wrt.Close();
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
            reader.Close();
        }

        private void BuildDefault() 
        {
            _setting.Add(new MenuOptions(MenuOptions.MUSIC).ToString(), DEFAULT_SETTING);
            _setting.Add(new MenuOptions(MenuOptions.SOUND).ToString(), DEFAULT_SETTING);
        }
    }
}
