using SaveSystem;
using UnityEditor;
using UnityEngine;

namespace Scenes.SaveSystem.Editor
{
    public static class SaveManagerEditor
    {
        [MenuItem("Tools/Save System/Open Save Folder")]
        private static void OpenSaveFolder() => 
            Application.OpenURL(SaveManager.SaveFolderPath);
    }
}