using SceneObjects;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class CameraChangeNode : GraphEditorNode
    {
        public CameraChangeNode(Vector2 position) : base(position) {}

        protected override VisualElement GetDataContainer()
        {
            VisualElement result = new();
            
            ObjectField objectField = new ObjectField()
            {
                allowSceneObjects = false,
                objectType = typeof(SceneReference),
                label = "Camera"
            };
            
            result.Add(objectField);

            return result;
        }
    }
}