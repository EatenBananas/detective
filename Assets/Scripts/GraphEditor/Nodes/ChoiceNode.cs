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
        private VisualElement _dataContainer;
        private readonly List<Choice> _options = new();
        private Label _label;
        private IntegerField _optionsCount;

        private VisualElement _choicesContainer;
        
        public ChoiceNode(string nodeName, Vector2 position) : base(nodeName, position,
            showOutputPort: false)
        {
            InitializeDataContainer();
        }

        public ChoiceNode(ChoiceNodeSave save) : this(save.NodeName, save.Position)
        {
            _optionsCount.value = save.Options.Count;
            _options = save.Options;
            RefreshOptions();
        }

        private void InitializeDataContainer()
        {
            _dataContainer = new VisualElement();
            
            _label = new Label("Options");
            _optionsCount = new IntegerField("Count");
            _optionsCount.RegisterValueChangedCallback((OptionsChangedCallback));

            _choicesContainer = new VisualElement();
            
            _dataContainer.Add(_label);
            _dataContainer.Add(_optionsCount);
            _dataContainer.Add(_choicesContainer);
        }

        protected override VisualElement GetDataContainer() => _dataContainer;

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

            RefreshOptions();
        }

        private void RefreshOptions()
        {
            _choicesContainer.Clear();

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
                
                _choicesContainer.Add(textField);
                _choicesContainer.Add(outcomePort);
            }
        }
    }
}