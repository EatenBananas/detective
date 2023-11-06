using System;
using UnityEngine;

namespace GraphEditor.Saves
{
    [Serializable]
    public class SetStateNodeSave : GraphEditorNodeSave
    {
        [field:SerializeField] public State State { get; set; }
        [field:SerializeField] public int SetTo { get; set; }
    }
}