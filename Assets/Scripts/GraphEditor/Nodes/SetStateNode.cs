using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class SetStateNode : GraphEditorNode
    {
        public SetStateNode(Vector2 position) : base(position) {}

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

            DropdownField equalToField = new DropdownField("Set to");
            equalToField.choices = _options;
            
            result.Add(stateField);
            result.Add(equalToField);
            
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