using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class StartNode : GraphEditorNode
    {
        public StartNode(Vector2 position) : base(position) {}

        protected override VisualElement GetDataContainer()
        {
            mainContainer.AddToClassList("start-node");
            inputContainer.Clear();
            
            return new VisualElement();
        }
    }
}