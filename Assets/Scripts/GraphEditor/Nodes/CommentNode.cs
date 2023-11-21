using GraphEditor.Saves;
using Interactions;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class CommentNode : GraphEditorNode
    {
        private VisualElement _dataContainer;
        private TextField _noteField;

        public CommentNode(string nodeName, Vector2 position) : base(nodeName, position,
            showInputPort: false, showOutputPort: false, showDescription: false)
        {
            mainContainer.AddToClassList("comment");
            InitializeDataContainer();
        }

        public CommentNode(CommentNodeSave save) : this(save.NodeName, save.Position)
        {
            SetBasicProperties(save);
            _noteField.value = save.Description;
        }

        private void InitializeDataContainer()
        {
            _dataContainer = new VisualElement();
            
            _noteField = new TextField()
            {
                multiline = true
            };
            _noteField.AddToClassList("ge__textfield");
            
            _dataContainer.Add(_noteField);
        }

        protected override VisualElement GetDataContainer() => _dataContainer;

        public override GraphEditorNodeSave ToSave()
        {
            CommentNodeSave save = new();
            FillBasicProperties(save);
            save.Description = _noteField.value;
            return save;
        }

        public override InteractionElement ToInteraction()
        {
            return null;
        }
    }
}