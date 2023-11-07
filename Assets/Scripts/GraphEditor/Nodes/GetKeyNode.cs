using GraphEditor.Saves;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class GetKeyNode : GraphEditorNode
    {
        private VisualElement _dataContainer;
        private EnumField _keyCodeField;

        public GetKeyNode(string nodeName, Vector2 position) : base(nodeName, position)
        {
            InitializeDataContainer();
        }

        public GetKeyNode(GetKeyNodeSave save) : this(save.NodeName, save.Position)
        {
            SetBasicProperties(save);
            _keyCodeField.value = save.KeyCode;
        }

        private void InitializeDataContainer()
        {
            _dataContainer = new VisualElement();
            
            _keyCodeField = new EnumField("Key",KeyCode.Escape);
            _dataContainer.Add(_keyCodeField);
        }

        protected override VisualElement GetDataContainer() => _dataContainer;

        public override GraphEditorNodeSave ToSave()
        {
            GetKeyNodeSave save = new();
            FillBasicProperties(save);

            save.KeyCode = _keyCodeField.value is KeyCode code ? code : KeyCode.None;

            return save;
        }
    }
}