using System;
using System.Collections.Generic;
using System.Linq;
using GraphEditor.Save;
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
        public string ID { get; } = Guid.NewGuid().ToString();
        public string NextNodeID { get; private set; }
        public string GroupID { get; private set; }
        protected abstract VisualElement GetDataContainer();

        private readonly bool _showInputPort;
        private readonly bool _showOutputPort;
        private readonly bool _showDescription;
        
        protected GraphEditorNode(Vector2 position,
            bool showInputPort = true, bool showOutputPort = true, bool showDescription = true)
        {
            
            _showInputPort = showInputPort;
            _showOutputPort = showOutputPort;
            _showDescription = showDescription;

            //capabilities ^= Capabilities.Resizable;
            
            Initialize(position);
            Draw();
        }

        private void Initialize(Vector2 position)
        {
            SetPosition(new Rect(position, Vector2.zero));
            styleSheets.Add((StyleSheet) EditorGUIUtility.Load("GraphEditor/GraphNodeStyles.uss"));
            
            extensionContainer.AddToClassList("ge__extension-container");
            mainContainer.AddToClassList("ge__main-container");
        }

        private void Draw()
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
                
                TextField description = new TextField()
                {
                    multiline = true,
                    value = "Description"
                };
                description.AddToClassList("ge__description");
                
                foldout.Add(description);
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

        public GraphEditorNodeSave ToSave()
        {
            GraphEditorNodeSave save = new()
            {
                ID = ID,
                NodeName = title,
                // GroupID = null, todo: implement
                Position = GetPosition().position,
                NextNodeID = NextNodeID
            };

            return save;

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
