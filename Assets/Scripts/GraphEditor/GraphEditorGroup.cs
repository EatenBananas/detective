using System;
using GraphEditor.Saves;
using UnityEditor.Experimental.GraphView;

namespace GraphEditor
{
    public class GraphEditorGroup : Group
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();
        public string OldTitle { get; set; }

        public GraphEditorGroupSave ToSave()
        {
            return new GraphEditorGroupSave()
            {
                ID = ID,
                GroupName = title,
                Position = GetPosition().position
            };
        }
    }
}