using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Scenes.SaveSystem.SecondSaveSystem
{
    public static class Save
    {
        public static bool IsProcessing { get; private set; }
        
        private static Dictionary<string, object> _gameData = new();


        public static async Task SaveData(string savePath)
        {
            if (IsProcessing) return;
            
            IsProcessing = true;

            await Task.Run(() =>
                File.WriteAllTextAsync(savePath, JsonConvert.SerializeObject(_gameData, Formatting.Indented)));
            
            IsProcessing = false;
        }

        public static async Task LoadData(string savePath)
        {
            if (IsProcessing) return;
            
            IsProcessing = true;

            await Task.Run(async () => _gameData = File.Exists(savePath)
                ? JsonConvert.DeserializeObject<Dictionary<string, object>>(await File.ReadAllTextAsync(savePath))
                : new Dictionary<string, object>());
            
            IsProcessing = false;
        }

        public static T GetKey<T>(string key, T defaultValue)
        {
            if (_gameData.TryGetValue(key, out var value))
                return value is JToken token ? token.ToObject<T>() : (T)value;

            return defaultValue;
        }
        
        public static void SetKey(string key, object value) => 
            _gameData[key] = value;

        public static void DeleteKey(string key) =>
            _gameData.Remove(key);
        
        public static bool IsKeyExists(string key) =>
            _gameData.ContainsKey(key);
    }
}