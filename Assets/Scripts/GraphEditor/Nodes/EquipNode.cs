using Equipment;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class EquipNode : GraphEditorNode
    {
        public EquipNode(Vector2 position) : base(position) {}

        protected override VisualElement GetDataContainer()
        {
            VisualElement result = new();

            ObjectField itemField = new ObjectField()
            {
                allowSceneObjects = false,
                objectType = typeof(Item),
                label = "Item"
            };
            
            result.Add(itemField);
            
            return result;
        }
    }
}