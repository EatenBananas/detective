using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SaveSystem.Generics
{
    public class Test : MonoBehaviour, ISavable
    {
        [SerializeField] private string _guid;
        [SerializeField] private int _int;
        [SerializeField] private string _string;
        [SerializeField] List<string> _list;
        [SerializeField] private Dictionary<string, int> _dictionary;
        
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_guid))
                _guid = Guid.NewGuid().ToString();
        }

        [Button]
        private void StartSave()
        {
            SaveManager.SaveGame();
        }
        
        [Button]
        private void StartLoad()
        {
            SaveManager.LoadGame();
        }
        
        
        public string Key => _guid;
        public object DefaultValue => 0;
        public int Priority => 0;
        
        public object Save()
        {
            return _list;
        }

        public void Load(object value)
        {
            var data = (List<string>)value;
            _list = data;
        }
    }
}