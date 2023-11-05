using System.Collections.Generic;
using UnityEngine;

namespace GraphEditor.Save
{
    public class GraphEditorSaveSO : ScriptableObject
    {
        [field:SerializeField] public string FileName { get; set; }
        [field:SerializeField] public List<GraphEditorNodeSave> Nodes { get; set; }
        [field:SerializeField] public List<GraphEditorGroupSave> Groups { get; set; }
        [field:SerializeField] public List<string> OldNodeNames { get; set; }
        [field:SerializeField] public List<string> OldGroupNames { get; set; }

        public void Initialize(string fileName)
        {
            FileName = fileName;
            Groups = new List<GraphEditorGroupSave>();
            Nodes = new List<GraphEditorNodeSave>();
        }
    }
}