using System;
using GraphEditor.Utils;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

namespace GraphEditor
{
    public class GraphEditorWindow : EditorWindow
    {
        private GraphEditorView _graphEditorView;
        private TextField _fileNameTextField;
        
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
            _graphEditorView = new GraphEditorView(this);
            
            _graphEditorView.StretchToParentSize();
            
            rootVisualElement.Add(_graphEditorView);
        }
        
        private void AddToolbar()
        {
            Toolbar toolbar = new();

            _fileNameTextField = new TextField("File Name:")
            {
                value = "NewFile"
                
            };
            _fileNameTextField.RegisterValueChangedCallback(evt =>
                _fileNameTextField.value = evt.newValue.RemoveSpecialCharacters().RemoveWhitespaces());
            
            Button saveButton = new(Save)
            {
                text = "Save",
                
            };

            Button loadButton = new(Load)
            {
                text = "Load"
            };
            
            Button clearButton = new(_graphEditorView.ClearGraph)
            {
                text = "Clear"
            };
            
            toolbar.Add(_fileNameTextField);
            toolbar.Add(saveButton);
            toolbar.Add(loadButton);
            toolbar.Add(clearButton);
            
            rootVisualElement.Add(toolbar);
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(_fileNameTextField.value))
            {
                EditorUtility.DisplayDialog(
                    "Invalid file name", "File name can't be empty", "ok");
                return;
            }
            
            GraphEditorIOUtils.Initialize(_graphEditorView, _fileNameTextField.value);
            GraphEditorIOUtils.Save();
        }

        private void Load()
        {
            GraphEditorIOUtils.Initialize(_graphEditorView, _fileNameTextField.value);
            GraphEditorIOUtils.Load();
        }
    }
}