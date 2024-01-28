using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Scenes.SaveSystem
{
    public static class SaveManager
    {
        public static string SaveFolderPath
        {
            get
            {
                var path = $"{Application.persistentDataPath}/Save";

                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);
                
                return path;
            }
        }
        public static string SaveFilePath
        {
            get
            {
                if (!string.IsNullOrEmpty(_saveFilePath))
                {
                    if (!System.IO.File.Exists(_saveFilePath))
                        System.IO.File.Create(_saveFilePath).Dispose();
                    
                    return _saveFilePath;
                }
                
                var saveFileName = $"{Guid.NewGuid()}.json";

                _saveFilePath = $"{SaveFolderPath}/{saveFileName}";
                
                if (!System.IO.File.Exists(_saveFilePath))
                    System.IO.File.Create(_saveFilePath).Dispose();

                return _saveFilePath;
            }
        }

        private static string _saveFilePath;
        
        public static void SaveGame()
        {
            var savableObjects = GetSavableObjects();
            
            savableObjects = SortSavableObjects(savableObjects);
            
            SaveSavableObjects(savableObjects);
        }

        public static void LoadGame()
        {
            var savableObjects = GetSavableObjects();
            
            savableObjects = SortSavableObjects(savableObjects);
            
            LoadSavableObjects(savableObjects);
        }

        public static void DeleteSaveKeysFromScene()
        {
            var savableObjects = GetSavableObjects();
            
            foreach (var savableObject in savableObjects)
                SaveFile.DeleteKey(savableObject.Key, SaveFilePath);
        }
        
        private static List<ISavable> GetSavableObjects() => 
            Object.FindObjectsOfType<MonoBehaviour>(true).OfType<ISavable>().ToList();

        private static List<ISavable> SortSavableObjects(IEnumerable<ISavable> savableObjects) =>
            savableObjects.OrderBy(savableObject => savableObject.Priority).ToList();

        private static void SaveSavableObjects(List<ISavable> savableObjects)
        {
            foreach (var savableObject in savableObjects)
                SaveFile.Save(savableObject.Key, savableObject.Save(), SaveFilePath);
        }

        private static void LoadSavableObjects(List<ISavable> savableObjects)
        {
            foreach (var savableObject in savableObjects)
            {
                var value = SaveFile.Load(savableObject.Key, savableObject.DefaultValue, SaveFilePath);
                
                savableObject.Load(value);
            }
        }
    }
}
