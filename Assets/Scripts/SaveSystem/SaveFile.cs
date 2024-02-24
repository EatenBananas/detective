using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SaveSystem
{
    public static class SaveFile
    {
        private static JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Formatting.Indented
        };
        
        public static bool IsKeyExists(string key, string filePath) => LoadData(filePath).ContainsKey(key);
        
        public static void DeleteKey(string key, string filePath)
        {
            var data = LoadData(filePath);

            if (data.ContainsKey(key))
                data.Remove(key);

            SaveData(data, filePath);
        }

        public static void Save(string key, object value, string filePath)
        {
            var data = LoadData(filePath) ?? new Dictionary<string, object> { { key, value } };

            data[key] = value;

            SaveData(data, filePath);
        }
        
        public static T Load<T>(string key, T defaultValue, string filePath)
        {
            var data = LoadData(filePath);

            if (data.TryGetValue(key, out var value)) return (T)value;

            return defaultValue;
        }
        
        private static void SaveData(Dictionary<string, object> data, string filePath) => 
            File.WriteAllText(filePath, JsonConvert.SerializeObject(data, _jsonSerializerSettings));

        private static Dictionary<string, object> LoadData(string filePath) =>
            File.Exists(filePath)
                ? JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(filePath), _jsonSerializerSettings)
                : new Dictionary<string, object>();
    }
}