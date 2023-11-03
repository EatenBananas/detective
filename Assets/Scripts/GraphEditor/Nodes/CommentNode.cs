using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class CommentNode : GraphEditorNode
    {
        public CommentNode(Vector2 position) : base(position) {}

        protected override VisualElement GetDataContainer()
        {
            inputContainer.Clear();
            outputContainer.Clear();
            
            mainContainer.AddToClassList("comment");
            
            VisualElement result = new();
            
            TextField noteField = new TextField()
            {
                multiline = true
            };
            noteField.AddToClassList("wide_text");

            result.Add(noteField);
            
            return result;
        }
    }
}