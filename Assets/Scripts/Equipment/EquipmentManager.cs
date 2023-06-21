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
    }
}
