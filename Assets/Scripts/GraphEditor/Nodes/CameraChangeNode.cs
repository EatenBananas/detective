using GraphEditor.Saves;
using SceneObjects;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class CameraChangeNode : GraphEditorNode
    {
        private ObjectField _cameraObjectField;
        public CameraChangeNode(Vector2 position) : base(position) {}

        protected override VisualElement GetDataContainer()
        {
            VisualElement result = new();
            
            _cameraObjectField = new ObjectField()
            {
                allowSceneObjects = false,
                objectType = typeof(SceneReference),
                label = "Camera"
            };
            
            result.Add(_cameraObjectField);

            return result;
        }

        public override GraphEditorNodeSave ToSave()
        {
            CameraChangeNodeSave save = new();
            FillBasicProperties(save);

            save.Camera = _cameraObjectField.value as SceneReference;
            
            return save;
        }
    }
}