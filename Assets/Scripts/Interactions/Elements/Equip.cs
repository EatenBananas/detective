using UnityEngine;
using System;
using Equipment;

namespace Interactions.Elements
{
    [Serializable]
    public class Equip : InteractionElement
    {
        [field:SerializeField] public Item Item { get; set; }
        public override void Execute()
        {
            EquipmentManager.Instance.Equip(Item);
            InteractionManager.Instance.CompleteElement();
        }
    }
}