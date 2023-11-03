using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class GetKeyNode : GraphEditorNode
    {
        public GetKeyNode(Vector2 position) : base(position) {}

        protected override VisualElement GetDataContainer()
        {
            VisualElement result = new();

            EnumField keyCodeField = new EnumField("Key",KeyCode.Escape);
            result.Add(keyCodeField);
            
            return result;
        }
    }
}