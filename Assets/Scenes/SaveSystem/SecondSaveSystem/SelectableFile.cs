using System.IO;
using UnityEngine;
using static System.Guid;

namespace Scenes.SaveSystem.SecondSaveSystem
{
    public class SelectableFile
    {
        public string FolderName { get; private set; }
        public string FolderPath { get; private set; }
        public string FileName
        {
            get
            {
                if (!string.IsNullOrEmpty(_fileName))
                    return _fileName;
                
                _fileName = $"{NewGuid()}.json";
                
                return _fileName;
            }
        }
        public string FilePath => $"{FolderPath}/{FileName}";
        public FileInfo FileInfo => new(FilePath);
        public int FilesInFolderCount => Directory.GetFiles(FolderPath).Length;

        private string _fileName;
        private string _filePath;

        public SelectableFile(string folderName)
        {
            FolderName = folderName;
            
            FolderPath = $"{Application.persistentDataPath}/{FolderName}";
            
            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);
        }
    }
}