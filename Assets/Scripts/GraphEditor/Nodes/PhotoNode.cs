using GraphEditor.Saves;
using SceneObjects;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class PhotoNode : GraphEditorNode
    {
        private ObjectField _photoField;
        private Toggle _visibleToggle;
        public PhotoNode(string nodeName, Vector2 position) : base(nodeName, position) {}
        protected override VisualElement GetDataContainer()
        {
            VisualElement result = new();

            _photoField = new ObjectField()
            {
                allowSceneObjects = false,
                objectType = typeof(SceneReference),
                label = "Scene Reference"
            };

            _visibleToggle = new Toggle("Visible");
            
            result.Add(_photoField);
            result.Add(_visibleToggle);

            return result;
        }

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