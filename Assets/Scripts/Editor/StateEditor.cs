using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace EditorUtilities
{
    [CustomEditor(typeof(State))]
    public class StateEditor : Editor
    {
        private State _state;

        private void OnEnable()
        {
            _state = target as State;
            Debug.Assert(_state != null, "_state != null");
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDefaultInspector();
            EditorGUILayout.LabelField("Initial State");
            _state.InitialState = EditorGUILayout.Popup(_state.InitialState, _state.States.ToArray());
            
            EditorUtility.SetDirty(_state);
            serializedObject.ApplyModifiedProperties();
        }
    }
}