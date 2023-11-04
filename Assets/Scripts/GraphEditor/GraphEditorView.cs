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
        private GraphEditorWindow _editorWindow;
        private GraphEditorSearchWindow _searchWindow;
        public GraphEditorView(GraphEditorWindow editorWindow)
        {
            _editorWindow = editorWindow;
            
            AddManipulators();
            AddGridBackground();
            AddSearchWindow();
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
            this.AddManipulator(CreateGroupContextualMenu());
        }
        
        private IManipulator CreateNodeContextualMenu()
        {
            ContextualMenuManipulator manipulator = new(MenuBuilder);
            return manipulator;
        }

        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator manipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Group",
                    actionEvent => CreateGroup(actionEvent.eventInfo.localMousePosition)));

            return manipulator;
        }
        
        private void MenuBuilder(ContextualMenuPopulateEvent obj)
        {
            foreach (var type in GraphEditorNode.SubTypes)
            {
                obj.menu.AppendAction($"Add {type.Name}",
                    actionEvent => CreateNode(type, actionEvent.eventInfo.localMousePosition));
            }
        }

        public void CreateNode(Type type, Vector2 mousePosition, bool isSearchWindow = false)
        {
            AddElement((GraphEditorNode) Activator.CreateInstance(type, GetLocalMousePosition(mousePosition, isSearchWindow)));
        }
        
        public void CreateGroup(Vector2 mousePosition, bool isSearchWindow = false)
        {
            Group group = new Group()
            {
                title = "Group"
            };
            group.SetPosition(new Rect(GetLocalMousePosition(mousePosition, isSearchWindow), Vector2.zero));
            
            AddElement(group);
        }
        
        private void AddSearchWindow()
        {
            if (_searchWindow == null)
            {
                _searchWindow = ScriptableObject.CreateInstance<GraphEditorSearchWindow>();
                _searchWindow.Initialize(this);
            }
            
            nodeCreationRequest = context =>
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
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

        private Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            if (isSearchWindow)
            {
                mousePosition -= _editorWindow.position.position;
            }
            
            return contentViewContainer.WorldToLocal(mousePosition);
        }
    }
}
