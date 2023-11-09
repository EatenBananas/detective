using System;
using System.Collections.Generic;
using GraphEditor.Nodes;
using UnityEngine;

namespace GraphEditor.Saves
{
    [Serializable]
    public class ChoiceNodeSave : GraphEditorNodeSave
    {
        [field:SerializeField] public List<Choice> Options { get; set; }
        public override GraphEditorNode ToNode() => new ChoiceNode(this);
    }
}