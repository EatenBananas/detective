using System;
using System.Collections.Generic;
using GraphEditor.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using Utils;

namespace GraphEditor
{
    public class GraphEditorView : GraphView
    {
        private GraphEditorWindow _editorWindow;
        private GraphEditorSearchWindow _searchWindow;

        private readonly SerializableDictionary<string, GraphEditorGroup> _groups = new();
        private readonly SerializableDictionary<string, GraphEditorNode> _nodes = new();
        
        public GraphEditorView(GraphEditorWindow editorWindow)
        {
            _editorWindow = editorWindow;

            deleteSelection = (operationName, user) => OnElementsDeleted();
            groupTitleChanged = (group, newTitle) => OnGroupRenamed((GraphEditorGroup)group);
            graphViewChanged = OnGraphViewChanged; 
            
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
            string nodeName = NextNodeName(type);
            
            GraphEditorNode node = (GraphEditorNode)Activator.CreateInstance(type,
                GetLocalMousePosition(mousePosition, isSearchWindow));

            node.title = nodeName;
            
            AddElement(node);
            _nodes[nodeName] = node;
        }
        
        public void CreateGroup(Vector2 mousePosition, bool isSearchWindow = false)
        {
            string groupName = NextGroupName();
            
            GraphEditorGroup group = new GraphEditorGroup()
            {
                title = groupName
            };
            group.OldTitle = groupName;
            group.SetPosition(new Rect(GetLocalMousePosition(mousePosition, isSearchWindow), Vector2.zero));

            foreach (GraphElement selectedElement in selection)
            {
                if (selectedElement is not GraphEditorNode)
                {
                    continue;
                }

                if (selectedElement is GraphEditorNode node)
                {
                    node.AddToGroup(group);
                    group.AddElement(node);
                }
            }
            
            AddElement(group);
            _groups[groupName] = group;
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

        private string NextGroupName(string prefix = "Group")
        {
            int i = 1;
            string groupName;

            do
            {
                groupName = $"{prefix}_{i}";
                i++;
            } while (_groups.ContainsKey(groupName));

            return groupName;
        }

        private string NextNodeName(Type type)
        {
            string typeName = type.Name.Replace("Node", "");
            string nodeName;
            int i = 1;
            
            do
            {
                nodeName = $"{typeName}_{i}";
                i++;
            } while (_nodes.ContainsKey(nodeName));

            return nodeName;
        }
        
        private void OnElementsDeleted()
        {
            List<GraphEditorNode> nodesToRemove = new();
            List<GraphEditorGroup> groupsToRemove = new();
            List<GraphElement> otherToRemove = new();
            
            foreach (GraphElement element in selection)
            {
                switch (element)
                {
                    case GraphEditorNode node:
                        nodesToRemove.Add(node);
                        break;
                    case GraphEditorGroup group:
                        groupsToRemove.Add(group);
                        break;
                    default:
                        otherToRemove.Add(element);
                        break;
                }
            }

            foreach (var group in groupsToRemove)
            {
                foreach (GraphElement element in group.containedElements)
                {
                    if (element is GraphEditorNode node)
                    {
                        nodesToRemove.Add(node);
                    }
                }
                
                _groups.Remove(group.title);
                RemoveElement(group);
            }
            
            foreach (GraphEditorNode node in nodesToRemove)
            {
                // todo: remove connections
                _nodes.Remove(node.title);
                RemoveElement(node);
            }

            foreach (GraphElement other in otherToRemove)
            {
                RemoveElement(other);
            }
        }

        private void OnGroupRenamed(GraphEditorGroup group)
        {
            string validatedTitle = group.title.RemoveWhitespaces().RemoveSpecialCharacters();

            if (validatedTitle != group.title)
            {
                group.title = validatedTitle;
                return; // preventing endless loop
            }
            
            if (_groups.ContainsKey(group.title))
            {
                group.title = NextGroupName(prefix: group.title);
            }
            
            _groups.Remove(group.OldTitle);
            _groups[group.title] = group;
            group.OldTitle = group.title;
        }
        
        private GraphViewChange OnGraphViewChanged(GraphViewChange changes)
        {
            if (changes.edgesToCreate != null)
            {
                foreach (Edge edge in changes.edgesToCreate)
                {
                    GraphEditorNode leftNode = (GraphEditorNode)edge.output.node;
                    GraphEditorNode rightNode = (GraphEditorNode)edge.input.node;

                    leftNode.ConnectTo(rightNode);
                }
            }

            if (changes.elementsToRemove != null)
            {
                Type edgeType = typeof(Edge);
                foreach (var element in changes.elementsToRemove)
                {
                    if (element.GetType() != edgeType)
                    {
                        continue;
                    }

                    Edge edge = (Edge)element;
                    GraphEditorNode node = (GraphEditorNode)edge.output.node;
                    node.Disconnect();
                    
                    RemoveElement(edge);
                }
            }
            
            return changes;
        }
        
    }
}
