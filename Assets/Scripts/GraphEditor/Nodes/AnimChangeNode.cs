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
    public class AnimChangeNode : GraphEditorNode
    {
        private VisualElement _dataContainer;
        private TextField _animNameTextField;
        public AnimChangeNode(string nodeName, Vector2 position ) : base(nodeName, position)
        {
            InitializeDataContainer();
        }
        
        public AnimChangeNode(AnimChangeNodeSave save) : this(save.NodeName, save.Position)
        {
            SetBasicProperties(save);
            _animNameTextField.value = save.AnimName;
        }

        private void InitializeDataContainer()
        {
            _dataContainer = new VisualElement();

            _animNameTextField = new TextField()
            {
                label = "Animation"
            };
            
            _dataContainer.Add(_animNameTextField);
        }

        protected override VisualElement GetDataContainer() => _dataContainer;

        public override GraphEditorNodeSave ToSave()
        {
            AnimChangeNodeSave save = new();
            FillBasicProperties(save);

            save.AnimName = _animNameTextField.value;

            return save;
        }

        public override InteractionElement ToInteraction()
        {
            var animChange = ScriptableObject.CreateInstance<AnimChange>();

            animChange.AnimName = _animNameTextField.value;

            return animChange;
        }
    }
}
#endif