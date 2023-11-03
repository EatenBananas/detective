using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor
{
    public class GraphEditorWindow : EditorWindow
    {
        [MenuItem("Window/Graph Editor")]
        public static void Open()
        {
            GetWindow<GraphEditorWindow>("Graph Editor");
        }

        private void OnEnable()
        {
            AddGraphView();
        }

        private void AddGraphView()
        {
            GraphEditorView view = new();
            
            view.StretchToParentSize();
            
            rootVisualElement.Add(view);
        }
    }
}