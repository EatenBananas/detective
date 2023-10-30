using GraphEditor.Elements;
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

        private GraphEditorNode CreateNode(Vector2 position)
        {
            GraphEditorNode node = new();
            
            node.Initialize(position);
            node.Draw();
            
            return node;
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
            ContextualMenuManipulator manipulator = new(
                menuEvent => menuEvent.menu.AppendAction("Add Node", actionEvent => AddElement(CreateNode(actionEvent.eventInfo.localMousePosition))));
            return manipulator;
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
