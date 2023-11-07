using GraphEditor.Saves;
using SceneObjects;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class TeleportNode : GraphEditorNode
    {
        private VisualElement _dataContainer;
        private ObjectField _locationField;

        public TeleportNode(string nodeName, Vector2 position) : base(nodeName, position)
        {
            InitializeDataContainer();
        }

        public TeleportNode(TeleportNodeSave save) : this(save.NodeName, save.Position)
        {
            _locationField.value = save.Location;
        }

        private void InitializeDataContainer()
        {
            _dataContainer = new VisualElement();
            
            _locationField = new ObjectField()
            {
                allowSceneObjects = false,
                objectType = typeof(SceneReference),
                label = "Location"
            };
            
            _dataContainer.Add(_locationField);
        }

        protected override VisualElement GetDataContainer() => _dataContainer;

        public override GraphEditorNodeSave ToSave()
        {
            TeleportNodeSave save = new();
            FillBasicProperties(save);
            
            save.Location = _locationField.value as SceneReference;

            return save;
        }
    }
}