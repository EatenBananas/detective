using GraphEditor.Saves;
using Interactions;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class StartNode : GraphEditorNode
    {
        private VisualElement _dataContainer;
        private TextField _titleTextField;
        
        public StartNode(string nodeName, Vector2 position) : base(nodeName, position, 
            showInputPort: false)
        {
            mainContainer.AddToClassList("start-node");
            InitializeDataContainer();
        }

        public StartNode(StartNodeSave save) : this(save.NodeName, save.Position)
        {
            SetBasicProperties(save);
            _titleTextField.value = save.Title;
        }
        
        private void InitializeDataContainer()
        {
            _dataContainer = new VisualElement();

            _titleTextField = new TextField
            {
                label = "Title",
                value = NodeName
            };
            _titleTextField.AddToClassList("ge__textfield");
            
            _dataContainer.Add(_titleTextField);
        }

        protected override VisualElement GetDataContainer() => _dataContainer;

        public override GraphEditorNodeSave ToSave()
        {
            NodeName = _titleTextField.value;
            StartNodeSave save = new()
            {
                Title = _titleTextField.value
            };
            FillBasicProperties(save);
            return save;
        }

        public override InteractionElement ToInteraction()
        {
            var start = ScriptableObject.CreateInstance<Interactions.Elements.StartInteraction>();
            
            start.Title = _titleTextField.value;
            
            return start;
        }
    }
}