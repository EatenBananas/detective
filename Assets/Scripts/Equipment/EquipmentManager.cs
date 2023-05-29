using System;
using System.Collections.Generic;
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
            UIManager.Instance.ReloadEquipment(_items);
        }

        public void Equip(Item item)
        {
            _items.Add(item);
            UIManager.Instance.ReloadEquipment(_items);
        }
    }
}
