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
        public ChoiceNode(string nodeName, Vector2 position) : base(nodeName, position, 
            showOutputPort: false) {}

        private readonly List<Choice> _options = new();
        
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
            ChoiceNodeSave save = new();
            FillBasicProperties(save);

            save.Options = _options;
            
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
                _options.Add(new Choice());
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
                var option = _options[i];
                
                TextField textField = new TextField()
                {
                    label = $"Option {i+1}",
                    value = _options[i].Text
                };

                textField.RegisterValueChangedCallback(evt => option.Text = evt.newValue);
                
                Port outcomePort = 
                    InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
                outcomePort.portName = "Outcome";

                outcomePort.userData = (Action<string>)(nodeID => option.NodeID = nodeID);
                
                extensionContainer.Add(textField);
                extensionContainer.Add(outcomePort);
            }
        }
    }
}