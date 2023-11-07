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
        private VisualElement _dataContainer;
        
        public CameraChangeNode(string nodeName, Vector2 position) : base(nodeName, position)
        {
            InitializeDataContainer();
        }

        public CameraChangeNode(CameraChangeNodeSave save) : this(save.NodeName, save.Position)
        {
            _cameraObjectField.value = save.Camera;

            ID = save.ID;
            GroupID = save.GroupID;
            NextNodeID = save.NextNodeID;
        }

        private void InitializeDataContainer()
        {
            _cameraObjectField = new ObjectField()
            {
                allowSceneObjects = false,
                objectType = typeof(SceneReference),
                label = "Camera"
            };

            _dataContainer = new VisualElement();
            _dataContainer.Add(_cameraObjectField);
        }

        protected override VisualElement GetDataContainer() => _dataContainer;

        public override GraphEditorNodeSave ToSave()
        {
            CameraChangeNodeSave save = new();
            FillBasicProperties(save);

            save.Camera = _cameraObjectField.value as SceneReference;
            
            return save;
        }
    }
}