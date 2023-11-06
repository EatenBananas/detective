using System;
using System.Collections.Generic;
using GraphEditor.Saves;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class ChoiceNode : GraphEditorNode
    {
        public ChoiceNode(Vector2 position) : base(position, 
            showOutputPort: false) {}

        private List<string> _options = new List<string>();
        
        protected override VisualElement GetDataContainer()
        {
            VisualElement result = new();

            Label label = new Label("Options");
            IntegerField optionsCount = new IntegerField("Count");
            optionsCount.RegisterValueChangedCallback((OptionsChangedCallback));
            
            result.Add(label);
            result.Add(optionsCount);

            return result;
        }

        public override GraphEditorNodeSave ToSave()
        {
            // todo: this one will be tough

            ChoiceNodeSave save = new();
            FillBasicProperties(save);

            return save;
        }

        private void OptionsChangedCallback(ChangeEvent<int> evt)
        {
            int count = evt.newValue;
            
            if (count < 0)
                return;

            while (_options.Count > count)
            {
                _options.RemoveAt(_options.Count-1);
            }

            while (_options.Count < count)
            {
                _options.Add(String.Empty);
            }

            Refresh();
        }

        private void Refresh()
        {
            for (int i = extensionContainer.childCount-1; i > 0; i--)
            {
                extensionContainer.RemoveAt(i);
            }

            for (int i = 0; i < _options.Count; i++)
            {
                TextField textField = new TextField()
                {
                    label = $"Option {i+1}",
                    value = _options[i]
                };
                
                Port outcomePort = 
                    InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
                outcomePort.portName = "Outcome";
                
                extensionContainer.Add(textField);
                extensionContainer.Add(outcomePort);
            }
        }
    }
}