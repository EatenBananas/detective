using System.Collections.Generic;
using System.Linq;
using GraphEditor.Saves;
using Interactions;
using Interactions.Elements;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class SetStateNode : GraphEditorNode
    {
        private readonly List<string> _options = new List<string>();

        private VisualElement _dataContainer;
        private ObjectField _stateField;
        private DropdownField _setToField;

        public SetStateNode(string nodeName, Vector2 position) : base(nodeName, position)
        {
            InitializeDataContainer();
        }

        public SetStateNode(SetStateNodeSave save) : this(save.NodeName, save.Position)
        {
            SetBasicProperties(save);
            _stateField.value = save.State;
            _setToField.index = save.SetTo;
        }

        void InitializeDataContainer()
        {
            _dataContainer = new VisualElement();
            
            _stateField = new ObjectField()
            {
                allowSceneObjects = false,
                objectType = typeof(State),
                label = "State"
            };

            _stateField.RegisterValueChangedCallback(StateValueChanged);

            _setToField = new DropdownField("Set to");
            _setToField.choices = _options;
            
            _dataContainer.Add(_stateField);
            _dataContainer.Add(_setToField);
        }

        protected override VisualElement GetDataContainer() => _dataContainer;

        public override GraphEditorNodeSave ToSave()
        {
            SetStateNodeSave save = new();
            FillBasicProperties(save);
            
            save.State = _stateField.value as State;
            save.SetTo = _setToField.index;

            return save;
        }

        public override InteractionElement ToInteraction()
        {
            var setState = ScriptableObject.CreateInstance<SetState>();

            setState.StateMachine = _stateField.value as State;
            setState.State = _setToField.index;

            return setState;
        }

        private void StateValueChanged(ChangeEvent<Object> evt)
        {
            State state = (State) evt.newValue;

            _options.Clear();
            foreach (var option in state.States)
            {
                _options.Add(option);
            }
            
            Refresh();
        }

        private void Refresh()
        {
            _setToField.choices = _options;
        }
    }
}