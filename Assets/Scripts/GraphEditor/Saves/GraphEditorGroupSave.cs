using System;
using UnityEngine;

namespace GraphEditor.Saves
{
    [Serializable]
    public class GraphEditorGroupSave
    {
        [field:SerializeField] public string GroupName { get; set; }
        [field:SerializeField] public string ID { get; set; }
        [field:SerializeField] public Vector2 Position { get; set; }
    }
}