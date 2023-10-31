using SceneObjects;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class TeleportNode : GraphEditorNode
    {
        public TeleportNode(Vector2 position) : base(position) {}

        protected override VisualElement GetDataContainer()
        {
            VisualElement result = new();

            ObjectField locationField = new ObjectField()
            {
                allowSceneObjects = false,
                objectType = typeof(SceneReference),
                label = "Location"
            };
            
            result.Add(locationField);
            return result;
        }
    }
}