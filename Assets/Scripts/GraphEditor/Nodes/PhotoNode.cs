using SceneObjects;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class PhotoNode : GraphEditorNode
    {
        public PhotoNode(Vector2 position) : base(position) {}
        protected override VisualElement GetDataContainer()
        {
            VisualElement result = new();

            ObjectField photoField = new ObjectField()
            {
                allowSceneObjects = false,
                objectType = typeof(SceneReference),
                label = "Scene Reference"
            };

            Toggle toggle = new Toggle("Visible");
            
            result.Add(photoField);
            result.Add(toggle);

            return result;
        }
    }
}