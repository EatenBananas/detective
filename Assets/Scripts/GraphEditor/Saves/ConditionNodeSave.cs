using System;
using GraphEditor.Nodes;
using SceneObjects;
using UnityEngine;

namespace GraphEditor.Saves
{
    [Serializable]
    public class ConditionNodeSave : GraphEditorNodeSave
    {
        [field:SerializeField] public State State { get; set; }
        [field:SerializeField] public int EqualTo { get; set; }
        [field:SerializeField] public string OutcomeNodeID { get; set; }
        public override GraphEditorNode ToNode() => new ConditionNode(this);
    }
}