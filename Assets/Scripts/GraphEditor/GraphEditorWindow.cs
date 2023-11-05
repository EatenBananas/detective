using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

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
            AddToolbar();
        }
        
        private void AddGraphView()
        {
            GraphEditorView view = new(this);
            
            view.StretchToParentSize();
            
            rootVisualElement.Add(view);
        }
        
        private void AddToolbar()
        {
            Toolbar toolbar = new();

            TextField fileNameTextField = new TextField("File Name:")
            {
                value = "NewFile"
                
            };
            fileNameTextField.RegisterValueChangedCallback(evt =>
                fileNameTextField.value = evt.newValue.RemoveSpecialCharacters().RemoveWhitespaces());
            Button saveButton = new()
            {
                text = "Save",
            };

            toolbar.Add(fileNameTextField);
            toolbar.Add(saveButton);
            
            rootVisualElement.Add(toolbar);
        }
    }
}