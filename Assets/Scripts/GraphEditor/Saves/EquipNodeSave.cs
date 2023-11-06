using System;
using Equipment;
using UnityEngine;

namespace GraphEditor.Saves
{
    [Serializable]
    public class EquipNodeSave : GraphEditorNodeSave
    {
        [field:SerializeField] public Item Item { get; set; }
    }
}