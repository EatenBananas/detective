using System;
using System.Collections.Generic;
using Interactions;
using SaveSystem;
using UnityEngine;

namespace Equipment
{
    public class EquipmentManager : MonoBehaviour, ISavable
    {
        public static EquipmentManager Instance { get; private set; }

        [field:SerializeField] private List<Item> _items = new List<Item>();

        [SerializeField] private string _guid;

        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            ReloadEquipment();
        }

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_guid))
                _guid = Guid.NewGuid().ToString();
        }
        
        public void Equip(Item item)
        {
            _items.Add(item);
            ReloadEquipment();
        }
        

        public void ReloadEquipment()
        {
            UIManager.Instance.ReloadEquipment(_items);
        }

        public void Use(ItemInteraction interaction, int slot)
        {
            interaction.Use(_items[slot]);
        }

        public void RemoveItem(int slot)
        {
            _items.RemoveAt(slot);
            ReloadEquipment();
        }

        public string Key => _guid;
        public object DefaultValue { get; }
        public int Priority { get; } = 0;
        public object Save()
        {
            return 10;
        }

        public void Load(object value)
        {
            //_items = (List<Item>)value;
        }
    }
}
