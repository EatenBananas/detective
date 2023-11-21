using GraphEditor.Nodes;

namespace GraphEditor.Saves
{
    public class CommentNodeSave : GraphEditorNodeSave
    {
        // no fields
        public override GraphEditorNode ToNode() => new CommentNode(this);
    }
}