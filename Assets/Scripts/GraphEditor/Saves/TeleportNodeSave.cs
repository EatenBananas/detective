#if UNITY_EDITOR
using System;
using GraphEditor.Nodes;
using SceneObjects;
using UnityEngine;

namespace GraphEditor.Saves
{
    [Serializable]
    public class TeleportNodeSave : GraphEditorNodeSave
    {
        [field:SerializeReference] public SceneReference Location { get; set; }
        public override GraphEditorNode ToNode() => new TeleportNode(this);
    }
}
#endif