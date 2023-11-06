using System.Collections.Generic;
using System.Linq;
using GraphEditor.Saves;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class SetStateNode : GraphEditorNode
    {
        private List<string> _options = new List<string>();

        private ObjectField _stateField;
        private DropdownField _setToField;
        
        public SetStateNode(Vector2 position) : base(position) {}
        
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

            _setToField = new DropdownField("Set to");
            _setToField.choices = _options;
            
            result.Add(_stateField);
            result.Add(_setToField);
            
            return result;
        }

        public override GraphEditorNodeSave ToSave()
        {
            SetStateNodeSave save = new();
            FillBasicProperties(save);
            
            save.State = _stateField.value as State;
            save.SetTo = _setToField.index;

            return save;
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