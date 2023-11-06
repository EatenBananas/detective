using System;
using System.Collections.Generic;
using UnityEngine;

namespace GraphEditor.Saves
{
    [Serializable]
    public class ChoiceNodeSave : GraphEditorNodeSave
    {
        [field:SerializeField] public List<Choice> Options { get; set; }
    }
}