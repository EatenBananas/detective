using System;
using System.Collections.Generic;
using Interactions;
using UnityEngine;

namespace Equipment
{
    public class EquipmentManager : MonoBehaviour
    {
        public static EquipmentManager Instance { get; private set; }

        [field:SerializeField] private List<Item> _items = new List<Item>();
        private int _activeSlot = -1;
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            ReloadEquipment();
        }

        public void Equip(Item item)
        {
            _items.Add(item);
            ReloadEquipment();
        }

        public void Select(int slot)
        {
            if (slot >= _items.Count)
            {
                return;
            }
            
            _activeSlot = slot == _activeSlot ? -1 : slot;
            ReloadEquipment();
        }

        private void ReloadEquipment()
        {
            UIManager.Instance.ReloadEquipment(_items, _activeSlot);
        }

        public void Use(ItemInteraction interaction)
        {
            interaction.Use(_items[_activeSlot]);
        }

        public void RemoveActiveItem()
        {
            _items.RemoveAt(_activeSlot);
            _activeSlot = -1;
            ReloadEquipment();
        }
    }
}
