using GraphEditor.Saves;
using SceneObjects;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class TeleportNode : GraphEditorNode
    {
        private ObjectField _locationField;
        public TeleportNode(Vector2 position) : base(position) {}

        protected override VisualElement GetDataContainer()
        {
            VisualElement result = new();

            _locationField = new ObjectField()
            {
                allowSceneObjects = false,
                objectType = typeof(SceneReference),
                label = "Location"
            };
            
            result.Add(_locationField);
            return result;
        }

        public override GraphEditorNodeSave ToSave()
        {
            TeleportNodeSave save = new();
            FillBasicProperties(save);
            
            save.Location = _locationField.value as SceneReference;

            return save;
        }
    }
}