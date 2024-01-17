#if UNITY_EDITOR
using GraphEditor.Saves;
using Interactions;
using Interactions.Elements;
using SceneObjects;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class CutsceneNode : GraphEditorNode
    {
        private VisualElement _dataContainer;
        private ObjectField _cutsceneField;
        public CutsceneNode(string nodeName, Vector2 position ) : base(nodeName, position)
        {
            InitializeDataContainer();
        }
        
        public CutsceneNode(CutsceneNodeSave save) : this(save.NodeName, save.Position)
        {
            SetBasicProperties(save);
            _cutsceneField.value = save.Cutscene;
        }

        private void InitializeDataContainer()
        {
            _dataContainer = new VisualElement();
        
            _cutsceneField = new ObjectField()
            {
                allowSceneObjects = false,
                objectType = typeof(SceneReference),
                label = "Cutscene"
            };
        
            _dataContainer.Add(_cutsceneField);
        }

        protected override VisualElement GetDataContainer() => _dataContainer;

        public override GraphEditorNodeSave ToSave()
        {
            CutsceneNodeSave save = new();
            FillBasicProperties(save);

            save.Cutscene = _cutsceneField.value as SceneReference;

            return save;
        }

        public override InteractionElement ToInteraction()
        {
            var cutscene = ScriptableObject.CreateInstance<Cutscene>();

            cutscene.Clip = _cutsceneField.value as SceneReference;

            return cutscene;
        }
    }
}
#endif