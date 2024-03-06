#if UNITY_EDITOR
using System;
using GraphEditor.Nodes;
using SceneObjects;
using UnityEngine;

namespace GraphEditor.Saves
{
    [Serializable]
    public class AnimChangeNodeSave : GraphEditorNodeSave
    {
        [field:SerializeReference] public string AnimName { get; set; }
        public override GraphEditorNode ToNode() => new AnimChangeNode(this);
    }
}
#endif