using System;
using UnityEngine;

namespace GraphEditor.Saves
{
    [Serializable]
    public abstract class GraphEditorNodeSave
    {
        public abstract GraphEditorNode ToNode();
        [field:SerializeField] public string NodeName { get; set; }
        [field:SerializeField] public string ID { get; set; }
        [field:SerializeField] public string NextNodeID { get; set; }
        [field:SerializeField] public string GroupID { get; set; }
        [field:SerializeField] public Vector2 Position { get; set; }
        
        [field:SerializeField] public string Description { get; set; }
    }
}