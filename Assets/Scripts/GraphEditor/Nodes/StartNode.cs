using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class StartNode : GraphEditorNode
    {
        public StartNode(Vector2 position) : base(position, showInputPort:false) {}

        protected override VisualElement GetDataContainer()
        {
            mainContainer.AddToClassList("start-node");
            
            return new VisualElement();
        }
    }
}