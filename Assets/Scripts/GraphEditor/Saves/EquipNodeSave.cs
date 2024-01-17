#if UNITY_EDITOR
using System;
using Equipment;
using GraphEditor.Nodes;
using UnityEngine;

namespace GraphEditor.Saves
{
    [Serializable]
    public class EquipNodeSave : GraphEditorNodeSave
    {
        [field:SerializeField] public Item Item { get; set; }
        public override GraphEditorNode ToNode() => new EquipNode(this);
    }
}
#endif