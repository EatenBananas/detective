using System;
using System.Collections.Generic;
using GraphEditor.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor
{
    public class GraphEditorView : GraphView
    {
        public GraphEditorView()
        {
            AddManipulators();
            AddGridBackground();
            AddStyles();
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new();

            ports.ForEach(port =>
            {
                if (startPort == port || startPort.node == port.node || startPort.direction == port.direction)
                {
                    return;
                }

                compatiblePorts.Add(port);
            });
            
            return compatiblePorts;
        }

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(CreateNodeContextualMenu());
        }

        private IManipulator CreateNodeContextualMenu()
        {
            ContextualMenuManipulator manipulator = new(MenuBuilder);
            return manipulator;
        }

        private void MenuBuilder(ContextualMenuPopulateEvent obj)
        {
            foreach (var type in GraphEditorNode.SubTypes)
            {
                // todo: reorganize this mess
                obj.menu.AppendAction($"Add {type.Name}", actionEvent => AddElement((GraphEditorNode) Activator.CreateInstance(type, actionEvent.eventInfo.localMousePosition)));
            }
        }

        private void AddStyles()
        {
            var styleSheet = (StyleSheet) EditorGUIUtility.Load("GraphEditor/GraphViewStyles.uss");
            styleSheets.Add(styleSheet);
        }

        private void AddGridBackground()
        {
            GridBackground background = new();
            background.StretchToParentSize();
            Insert(0, background);
        }
    }
}
