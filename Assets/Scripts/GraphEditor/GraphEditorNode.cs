using System;
using System.Collections.Generic;
using System.Linq;
using GraphEditor.Saves;
using Interactions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor
{
    public abstract class GraphEditorNode : Node
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();
        public string NextNodeID { get; set; }
        public string GroupID { get; set; }
        protected abstract VisualElement GetDataContainer();
        public abstract GraphEditorNodeSave ToSave();

        private readonly bool _showInputPort;
        private readonly bool _showOutputPort;
        private readonly bool _showDescription;

        private TextField _descriptionTextField;
        
        protected GraphEditorNode(string nodeName, Vector2 position,
            bool showInputPort = true, bool showOutputPort = true, bool showDescription = true)
        {
            title = nodeName;
            
            _showInputPort = showInputPort;
            _showOutputPort = showOutputPort;
            _showDescription = showDescription;

            //capabilities ^= Capabilities.Resizable;
            
            Initialize(position);
        }

        private void Initialize(Vector2 position)
        {
            SetPosition(new Rect(position, Vector2.zero));
            styleSheets.Add((StyleSheet) EditorGUIUtility.Load("GraphEditor/GraphNodeStyles.uss"));
            
            extensionContainer.AddToClassList("ge__extension-container");
            mainContainer.AddToClassList("ge__main-container");
        }

        public void Draw()
        {
            Label label = new(title);
            //label.name = _title;
            titleContainer.Insert(0, label);
            
            if (_showDescription)
            {
                VisualElement customDataContainer = new();
                
                customDataContainer.AddToClassList("ge__custom-data-container");
                
                Foldout foldout = new Foldout
                {
                    value = false,
                    //text = "Description"
                };
                
                foldout.AddToClassList("ge__foldout");
                
                _descriptionTextField = new TextField()
                {
                    multiline = true,
                    value = "Description"
                };
                _descriptionTextField.AddToClassList("ge__description");
                
                foldout.Add(_descriptionTextField);
                customDataContainer.Add(foldout);
                
                //mainContainer.Insert(1, customDataContainer);
                extensionContainer.Add(customDataContainer);
            }

            if (_showInputPort)
            {
                Port inputPort =
                    InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
                inputPort.portName = "Previous";
                inputContainer.Add(inputPort);
            }

            if (_showOutputPort)
            {
                Port outputPort =
                    InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                outputPort.portName = "Next";
                outputContainer.Add(outputPort);
            }

            extensionContainer.Add(GetDataContainer());
            
            RefreshExpandedState();
        }
        
        // todo: przerobić na generica
        private static List<Type> _subTypes;
        public static List<Type> SubTypes =>
            _subTypes ??= (AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes(), (assembly, type) => new {assembly, type})
                .Where(@t => @t.type.IsSubclassOf(typeof(GraphEditorNode)))
                .Select(@t => @t.type)).ToList();
        
        public void FillBasicProperties(GraphEditorNodeSave save)
        {
            save.ID = ID;
            save.NodeName = title;
            save.GroupID = GroupID;
            save.Position = GetPosition().position;
            save.NextNodeID = NextNodeID;
            save.Description = _descriptionTextField != null ? _descriptionTextField.value : string.Empty;
        }

        public void ConnectTo(GraphEditorNode node)
        {
            NextNodeID = node.ID;
        }

        public void Disconnect()
        {
            NextNodeID = string.Empty;
        }

        public void AddToGroup(GraphEditorGroup group)
        {
            GroupID = group.ID;
        }
    }
}
