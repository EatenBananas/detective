#if UNITY_EDITOR
using System;
using GraphEditor.Nodes;
using UnityEngine;

namespace GraphEditor.Saves
{
    [Serializable]
    public class StartNodeSave : GraphEditorNodeSave
    {
        [field:SerializeField] public string Title { get; set; }
        public override GraphEditorNode ToNode() => new StartNode(this);
    }
}
#endif