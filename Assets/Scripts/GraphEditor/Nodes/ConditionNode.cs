using System.Collections.Generic;
using System.Linq;
using Interactions;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class ConditionNode : GraphEditorNode
    {
        public ConditionNode(Vector2 position) : base(position) {}

        private List<string> _options = new List<string>();

        protected override VisualElement GetDataContainer()
        {
            VisualElement result = new();

            ObjectField stateField = new ObjectField()
            {
                allowSceneObjects = false,
                objectType = typeof(State),
                label = "State"
            };

            stateField.RegisterValueChangedCallback(StateValueChanged);

            DropdownField equalToField = new DropdownField("Equal to");
            equalToField.choices = _options;

            Port interactionPort = 
                InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            interactionPort.portName = "Interaction";
            
            result.Add(stateField);
            result.Add(equalToField);
            result.Add(interactionPort);
            
            return result;
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
            // todo: remove magic number
            if (extensionContainer.childCount < 2)
                return;
            
            DropdownField dropdown = (DropdownField) extensionContainer.Children().ToArray()[1];
            
            dropdown.choices = _options;
        }
    }
}