#if UNITY_EDITOR
using System;
using GraphEditor.Nodes;
using SceneObjects;
using UnityEngine;

namespace GraphEditor.Saves
{
    [Serializable]
    public class CutsceneNodeSave : GraphEditorNodeSave
    {
        [field:SerializeReference] public SceneReference Cutscene { get; set; }
        public override GraphEditorNode ToNode() => new CutsceneNode(this);
    }
}
#endif