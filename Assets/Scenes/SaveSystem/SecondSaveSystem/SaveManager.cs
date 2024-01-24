using System.Collections.Generic;

namespace Scenes.SaveSystem.SecondSaveSystem
{
    public static class SaveManager
    {
        public static readonly SelectableFile SelectedSaveFile = new("Saves");
        public static List<SavableBehaviour> SavableBehaviours { get; private set; } = new();

        public static async void SaveEntireGameData()
        {
            if (Save.IsProcessing) return;
            
            RefreshGameData();

            await Save.SaveData(SelectedSaveFile.FilePath);
        }
        
        public static async void LoadEntireGameData()
        {
            if (Save.IsProcessing) return;
            
            await Save.LoadData(SelectedSaveFile.FilePath);

            foreach (var savableBehaviour in SavableBehaviours)
                savableBehaviour.SetValue(Save.GetKey(savableBehaviour.Key, savableBehaviour.DefaultValue));
        }

        public static async void BackgroundSave() => 
            await Save.SaveData(SelectedSaveFile.FilePath);

        public static async void BackgroundLoad() => 
            await Save.LoadData(SelectedSaveFile.FilePath);

        private static Dictionary<string, object> GetLatestData()d
        {
            var saveData = new Dictionary<string, object>();

            foreach (var savableBehaviour in SavableBehaviours)
                saveData[savableBehaviour.Key] = savableBehaviour.GetValue();

            return saveData;
        }
        
        private static void RefreshGameData()
        {
            var latestData = GetLatestData();

            foreach (var newData in latestData) 
                SecondSaveSystem.Save.SetKey(newData.Key, newData.Value);
        }
    }
}