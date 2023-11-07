using System;
using System.Collections.Generic;
using System.Linq;
using GraphEditor.Saves;
using Interactions;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
//using Object = UnityEngine.Object;

namespace GraphEditor.Nodes
{
    public class ConditionNode : GraphEditorNode
    {
        private VisualElement _dataContainer;
        
        private List<string> _options = new List<string>();

        private DropdownField _equalToField;
        private ObjectField _stateField;
        private String _outcomeNodeID;

        public ConditionNode(string nodeName, Vector2 position) : base(nodeName, position)
        {
            InitializeDataContainer();
        }

        public ConditionNode(ConditionNodeSave save) : this(save.NodeName, save.Position)
        {
            SetBasicProperties(save);
            _stateField.value = save.State;
            _equalToField.index = save.EqualTo;
            _outcomeNodeID = save.OutcomeNodeID;
        }

        private void InitializeDataContainer()
        {
            _dataContainer = new VisualElement();
            
            _stateField = new ObjectField()
            {
                allowSceneObjects = false,
                objectType = typeof(State),
                label = "State"
            };

            _stateField.RegisterValueChangedCallback(StateValueChanged);

            _equalToField = new DropdownField("Equal to");
            _equalToField.choices = _options;

            Port interactionPort = 
                InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            interactionPort.portName = "Interaction";

            // binding custom action
            interactionPort.userData = (Action<string>)(nodeID => _outcomeNodeID = nodeID);
            
            _dataContainer.Add(_stateField);
            _dataContainer.Add(_equalToField);
            _dataContainer.Add(interactionPort);
        }

        protected override VisualElement GetDataContainer() => _dataContainer;

        private void StateValueChanged(ChangeEvent<UnityEngine.Object> evt)
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
            _equalToField.choices = _options;
        }
        
        public override GraphEditorNodeSave ToSave()
        {
            ConditionNodeSave save = new();
            FillBasicProperties(save);

            save.State = _stateField.value as State;
            save.EqualTo = _equalToField.index;
            save.OutcomeNodeID = _outcomeNodeID;

            return save;
        }
    }
}