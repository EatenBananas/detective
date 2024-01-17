#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using GraphEditor.Nodes;
using SceneObjects;
using UnityEngine;

namespace GraphEditor.Saves
{
    [Serializable]
    public class ConditionNodeSave : GraphEditorNodeSave
    {
        [field:SerializeField] public State State { get; set; }
        [field:SerializeField] public List<OutcomeSave> Outcomes { get; set; }
        public override GraphEditorNode ToNode() => new ConditionNode(this);
    }
}
#endif