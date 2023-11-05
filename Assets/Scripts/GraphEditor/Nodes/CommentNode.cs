using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class CommentNode : GraphEditorNode
    {
        public CommentNode(Vector2 position) : base(position,
            showInputPort:false, showOutputPort:false, showDescription:false) {}
        
        protected override VisualElement GetDataContainer()
        {
            mainContainer.AddToClassList("comment");
            
            VisualElement result = new();
            
            TextField noteField = new TextField()
            {
                multiline = true
            };
            noteField.AddToClassList("ge__textfield");

            result.Add(noteField);
            
            return result;
        }
    }
}