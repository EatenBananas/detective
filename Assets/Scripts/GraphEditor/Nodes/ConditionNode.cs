#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using GraphEditor.Saves;
using GraphEditor.Utils;
using Interactions;
using Interactions.Elements;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;
using Utils;

//using Object = UnityEngine.Object;

namespace GraphEditor.Nodes
{
    public class ConditionNode : GraphEditorNode
    {
        private VisualElement _dataContainer;
        
        private readonly List<OutcomeSave> _outcomes = new();
        private readonly Dictionary<OutcomeSave, Port> _ports = new();
        
        private List<string> _options = new List<string>();

        private VisualElement _outcomesContainer;
        private ObjectField _stateField;
        private IntegerField _optionsCount;
        private Label _label;
        
        //private DropdownField _equalToField;
        //private string _outcomeNodeID;
        //private Port _outcomePort;

        public ConditionNode(string nodeName, Vector2 position) : base(nodeName, position)
        {
            InitializeDataContainer();
        }

        public ConditionNode(ConditionNodeSave save) : this(save.NodeName, save.Position)
        {
            SetBasicProperties(save);
            _stateField.value = save.State;
            RefreshState(save.State);
            _optionsCount.value = save.Outcomes.Count;
            _outcomes = save.Outcomes;
            RefreshOutcomes();
        }

        public override List<Edge> LoadConnections()
        {
            List<Edge> edges = base.LoadConnections() ?? new List<Edge>();

            foreach (var outcome in _outcomes)
            {
                Port port = _ports[outcome];

                if (port == null)
                {
                    Debug.LogError("Port not found?!");
                    continue;
                }

                GraphEditorNode nextNode = GraphEditorIOUtils.GetNode(outcome.NodeID);
                
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
            
            _stateField = new ObjectField()
            {
                allowSceneObjects = false,
                objectType = typeof(State),
                label = "State"
            };
            
            _stateField.RegisterValueChangedCallback(StateValueChanged);

            _label = new Label("Outcomes");
            _optionsCount = new IntegerField("Count");
            _optionsCount.RegisterValueChangedCallback((OutcomesChangedCallback));

            _outcomesContainer = new VisualElement();
            
            _dataContainer.Add(_stateField);
            _dataContainer.Add(_label);
            _dataContainer.Add(_optionsCount);
            _dataContainer.Add(_outcomesContainer);
        }

        protected override VisualElement GetDataContainer() => _dataContainer;

        private void StateValueChanged(ChangeEvent<UnityEngine.Object> evt)
        {
            State state = (State) evt.newValue;
            RefreshState(state);
        }

        private void RefreshState(State state)
        {
            _options.Clear();
            foreach (var option in state.States)
            {
                _options.Add(option);
            }
        }
        
        public override GraphEditorNodeSave ToSave()
        {
            ConditionNodeSave save = new();
            FillBasicProperties(save);

            save.State = _stateField.value as State;
            save.Outcomes = _outcomes;

            return save;
        }

        public override InteractionElement ToInteraction()
        {
            var condition = ScriptableObject.CreateInstance<Condition>();

            condition.StateMachine = _stateField.value as State;

            return condition;
        }
        
        public override void UpdateConnections(InteractionElement element)
        {
            base.UpdateConnections(element);

            if (element is not Condition condition)
            {
                Debug.LogError("Wrong interaction type, Choice expected");
                return;
            }
            
            condition.Outcomes = new SerializableDictionary<int, InteractionElement>();

            foreach (var outcome in _outcomes)
            {
                condition.Outcomes[outcome.Value] = GraphEditorIOUtils.GetElement(outcome.NodeID);
            }
        }
        
        
        
        private void OutcomesChangedCallback(ChangeEvent<int> evt)
        {
            int count = evt.newValue;
            
            if (count < 0)
                return;

            while (_outcomes.Count > count)
            {
                _outcomes.RemoveAt(_options.Count-1);
            }

            while (_outcomes.Count < count)
            {
                _outcomes.Add(new OutcomeSave());
            }

            RefreshOutcomes();
        }

        private void RefreshOutcomes()
        {
            _outcomesContainer.Clear();
            _ports.Clear();

            for (int i = 0; i < _outcomes.Count; i++)
            {
                var outcome = _outcomes[i];
                
                var equalToField = new DropdownField($"Outcome {i+1}");
                equalToField.choices = _options;
                equalToField.index = outcome.Value;
                
                equalToField.RegisterValueChangedCallback(evt => outcome.Value = equalToField.index);
                
                Port outcomePort = 
                    InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
                outcomePort.portName = "Outcome";
                outcomePort.userData = (Action<string>)(nodeID => outcome.NodeID = nodeID);
                
                _ports[outcome] = outcomePort;
                
                _outcomesContainer.Add(equalToField);
                _outcomesContainer.Add(outcomePort);
            }
        }
    }
}
#endif