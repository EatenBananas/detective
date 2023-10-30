using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Elements
{
    public class GraphEditorNode : Node
    {
        public string InteractionName { get; set; }
        public string Text { get; set; }
        public Type InteractionType { get; set; }

        public void Initialize(Vector2 position)
        {
            InteractionName = "InteractionName";
            Text = "example text";
            
            SetPosition(new Rect(position, Vector2.zero));
        }

        public void Draw()
        {
            TextField interactionNameTextField = new()
            {
                value = InteractionName
            };
            titleContainer.Insert(0, interactionNameTextField);

            Port inputPort =
                InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = "Input";
            inputContainer.Add(inputPort);

            VisualElement customDataContainer = new VisualElement();
            
            Foldout textFoldout = new Foldout()
            {
                text = "Blabla"
            };

            TextField textTextField = new TextField()
            {
                value = Text
            };
            
            textFoldout.Add(textTextField);
            customDataContainer.Add(textFoldout);
            
            extensionContainer.Add(customDataContainer);
            
            RefreshExpandedState();
        }
    }
}
