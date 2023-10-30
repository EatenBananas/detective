using System;
using System.Collections.Generic;
using System.Linq;
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
        protected abstract VisualElement GetDataContainer();
        protected GraphEditorNode(Vector2 position)
        {
            Initialize(position);
            Draw();
        }
        
        private void Initialize(Vector2 position)
        {
            // todo: pozycja nie uwzględnia skali!
            SetPosition(new Rect(position, Vector2.zero));
            styleSheets.Add((StyleSheet) EditorGUIUtility.Load("GraphEditor/GraphNodeStyles.uss"));
        }

        private void Draw()
        {
            Label label = new(GetType().Name);
            label.name = "title";
            titleContainer.Insert(0, label);

            // todo: ogarnąć to
            // TextField description = new TextField()
            // {
            //     multiline = true,
            //     label = "Description"
            // };
            // titleContainer.Add(description);

            Port inputPort =
                InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
            inputPort.portName = "Previous";
            inputContainer.Add(inputPort);
            
            Port outputPort =
                InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            outputPort.portName = "Next";
            outputContainer.Add(outputPort);

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
    }
}
