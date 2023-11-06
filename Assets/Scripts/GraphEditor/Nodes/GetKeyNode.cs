using GraphEditor.Saves;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class GetKeyNode : GraphEditorNode
    {
        private EnumField _keyCodeField;
        public GetKeyNode(Vector2 position) : base(position) {}

        protected override VisualElement GetDataContainer()
        {
            VisualElement result = new();

            _keyCodeField = new EnumField("Key",KeyCode.Escape);
            result.Add(_keyCodeField);
            
            return result;
        }

        public override GraphEditorNodeSave ToSave()
        {
            GetKeyNodeSave save = new();
            FillBasicProperties(save);

            save.KeyCode = _keyCodeField.value is KeyCode code ? code : KeyCode.None;

            return save;
        }
    }
}