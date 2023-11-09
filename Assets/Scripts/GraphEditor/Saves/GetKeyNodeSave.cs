using System;
using GraphEditor.Nodes;
using UnityEngine;

namespace GraphEditor.Saves
{
    [Serializable]
    public class GetKeyNodeSave : GraphEditorNodeSave
    {
        [field:SerializeField] public KeyCode KeyCode { get; set; }
        public override GraphEditorNode ToNode() => new GetKeyNode(this);
    }
}