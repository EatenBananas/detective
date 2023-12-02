using System;
using System.Collections.Generic;
using GraphEditor.Saves;
using GraphEditor.Utils;
using Interactions;
using Interactions.Elements;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class ChoiceNode : GraphEditorNode
    {
        private VisualElement _dataContainer;
        private readonly List<OptionSave> _options = new();
        private readonly Dictionary<OptionSave, Port> _ports = new();
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
            SetBasicProperties(save);
            _optionsCount.value = save.Options.Count;
            _options = save.Options;
            RefreshOptions();
        }

        public override List<Edge> LoadConnections()
        {
            List<Edge> edges = base.LoadConnections() ?? new List<Edge>();

            foreach (var option in _options)
            {
                Port port = _ports[option];

                if (port == null)
                {
                    Debug.LogError("Port not found?!");
                    continue;
                }

                GraphEditorNode nextNode = GraphEditorIOUtils.GetNode(option.NodeID);
                
                if (nextNode == null)
                    continue;
                
                Edge edge = port.ConnectTo(nextNode.InputPort);
                
                edges.Add(edge);
            }

            return edges;
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

        public override InteractionElement ToInteraction()
        {
            var choice = ScriptableObject.CreateInstance<Interactions.Elements.Choice>();
            
            return choice;
        }

        public override void UpdateConnections(InteractionElement element)
        {
            base.UpdateConnections(element);

            if (element is not Interactions.Elements.Choice choice)
            {
                Debug.LogError("Wrong interaction type, Choice expected");
                return;
            }
            
            choice.Options = new List<Option>();

            foreach (var connection in _options)
            {
                choice.Options.Add(new Option()
                {
                    Text = connection.Text,
                    Outcome = GraphEditorIOUtils.GetElement(connection.NodeID) 
                });
            }

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
                _options.Add(new OptionSave());
            }

            RefreshOptions();
        }

        private void RefreshOptions()
        {
            _choicesContainer.Clear();
            _ports.Clear();

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
                
                _ports[option] = outcomePort;
                
                _choicesContainer.Add(textField);
                _choicesContainer.Add(outcomePort);
            }
        }
    }
}