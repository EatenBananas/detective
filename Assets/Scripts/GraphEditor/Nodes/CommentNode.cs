using GraphEditor.Saves;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class CommentNode : GraphEditorNode
    {
        private TextField _noteField;
        public CommentNode(Vector2 position) : base(position,
            showInputPort:false, showOutputPort:false, showDescription:false) {}
        
        protected override VisualElement GetDataContainer()
        {
            mainContainer.AddToClassList("comment");
            
            VisualElement result = new();
            
            _noteField = new TextField()
            {
                multiline = true
            };
            _noteField.AddToClassList("ge__textfield");

            result.Add(_noteField);
            
            return result;
        }

        public override GraphEditorNodeSave ToSave()
        {
            CommentNodeSave save = new();
            FillBasicProperties(save);
            save.Description = _noteField.value;
            return save;
        }
    }
}