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
        public ConditionNode(Vector2 position) : base(position) {}

        private List<string> _options = new List<string>();

        private DropdownField _equalToField;
        private ObjectField _stateField;
        private String _outcomeNodeID;  // todo: implement logic for it
        
        protected override VisualElement GetDataContainer()
        {
            VisualElement result = new();

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
            
            result.Add(_stateField);
            result.Add(_equalToField);
            result.Add(interactionPort);
            
            return result;
        }

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