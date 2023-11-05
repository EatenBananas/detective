using UnityEditor.Experimental.GraphView;

namespace GraphEditor
{
    public class GraphEditorGroup : Group
    {
        public string OldTitle { get; set; }
        
        public GraphEditorGroup()
        {
            OldTitle = title;
        }
    }
}