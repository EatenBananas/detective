using GraphEditor.Saves;
using SceneObjects;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class PhotoNode : GraphEditorNode
    {
        private VisualElement _dataContainer;
        private ObjectField _photoField;
        private Toggle _visibleToggle;

        public PhotoNode(string nodeName, Vector2 position) : base(nodeName, position)
        {
            InitializeDataContainer();
        }

        public PhotoNode(PhotoNodeSave save) : this(save.NodeName, save.Position)
        {
            SetBasicProperties(save);
            _photoField.value = save.Picture;
            _visibleToggle.value = save.Visible;
        }

        private void InitializeDataContainer()
        {
            _dataContainer = new VisualElement();
            
            _photoField = new ObjectField()
            {
                allowSceneObjects = false,
                objectType = typeof(SceneReference),
                label = "Scene Reference"
            };

            _visibleToggle = new Toggle("Visible");
            
            _dataContainer.Add(_photoField);
            _dataContainer.Add(_visibleToggle);
        }

        protected override VisualElement GetDataContainer() => _dataContainer;
        public override GraphEditorNodeSave ToSave()
        {
            PhotoNodeSave save = new();
            FillBasicProperties(save);
            
            save.Picture = _photoField.value as SceneReference;
            save.Visible = _visibleToggle.value;

            return save;
        }
    }
}