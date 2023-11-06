using System;
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
    }
}