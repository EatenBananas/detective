#if UNITY_EDITOR
using System;
using GraphEditor.Nodes;
using UnityEngine;

namespace GraphEditor.Saves
{
    [Serializable]
    public class DialogueNodeSave : GraphEditorNodeSave
    {
        [field:SerializeField] public string DialogueText { get; set; }
        [field:SerializeField] public DialogueNpc DialogueNpc { get; set; }
        
        [field:SerializeField] public string DialoguePath { get; set; }
        public override GraphEditorNode ToNode() => new DialogueNode(this);
    }
}
#endif