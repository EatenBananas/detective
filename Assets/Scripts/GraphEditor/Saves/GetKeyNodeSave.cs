using System;
using UnityEngine;

namespace GraphEditor.Saves
{
    [Serializable]
    public class GetKeyNodeSave : GraphEditorNodeSave
    {
        [field:SerializeField] public KeyCode KeyCode { get; set; }
    }
}