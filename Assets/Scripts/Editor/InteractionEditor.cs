using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interactions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(Interaction))]
// ReSharper disable once CheckNamespace
public class InteractionEditor : Editor
{
    private Interaction _interaction;
    private ReorderableList _reorderableList;

    private const string HEADER_LABEL = "Interaction elements";
    
    private void OnEnable()
    {
        _interaction = target as Interaction;
        Debug.Assert(_interaction != null, nameof(_interaction) + " != null");
        _reorderableList = new ReorderableList(_interaction.Elements, typeof(Interaction))
        {
            drawHeaderCallback = rect => { EditorGUI.LabelField(rect, HEADER_LABEL); },
            // drawFooterCallback = null,
            drawElementCallback = DrawListItem,
            // drawElementBackgroundCallback = null,
            // drawNoneElementCallback = null,
            elementHeightCallback = delegate { return EditorGUIUtility.singleLineHeight * 5; },
            // onReorderCallbackWithDetails = null,
            // onReorderCallback = null,
            // onSelectCallback = null,
            // onAddCallback = null,
            // onAddDropdownCallback = null,
            // onRemoveCallback = null,
            // onMouseDragCallback = null,
            // onMouseUpCallback = null,
            // onCanRemoveCallback = null,
            // onCanAddCallback = null,
            // onChangedCallback = null,
            // displayAdd = true,
            // displayRemove = true,
            // elementHeight = 0,
            // headerHeight = 0,
            // footerHeight = 0,
            // showDefaultBackground = false,
            //serializedProperty = null,
            // list = null,
            // index = 0,
            // multiSelect = false,
            // draggable = false
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawInteractionGUI();
        
        EditorUtility.SetDirty(_interaction);
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawInteractionGUI()
    {
        _reorderableList.DoLayoutList();
    }
    
    private void DrawListItem(Rect rect, int index, bool isActive, bool isFocused)
    {
        var element = _reorderableList.list[index] as InteractionElementData;
        Debug.Assert(element != null, "element != null");
        var editorUtilities = new Utilities(rect.x, rect.y);

        editorUtilities.TopMargin();
        
        element.Type = (InteractionElementType) editorUtilities.EnumField(element.Type, "Type");

        // temp
        switch (element.Type)
        {
            case InteractionElementType.CHANGE_CAMERA:
            {
                //Debug.Assert(CameraManager.Instance != null, "CameraManager.Instance != null");
                //temp find

                var cm = FindObjectOfType<CameraManager>();
                
                //element.Number1 = editorUtilities.IntField(element.Number1, "Camera ID");
                element.Number1 = editorUtilities.IntPopupField(element.Number1, "Camera",
                    //new[] {"Main", "Mailbox", "Receptionist"}, new[] {0, 1, 2});
                    cm.GetCameraNames(),
                    cm.GetCameraIndexes());
                break;
            }
            case InteractionElementType.GET_KEY:
            {
                element.KeyCode = (KeyCode) editorUtilities.EnumField(element.KeyCode, "Key");
                break;
            }
            case InteractionElementType.DIALOGUE:
            {
                element.Text1 = editorUtilities.TextField(element.Text1, "NPC Name");
                element.Text2 = editorUtilities.TextField(element.Text2, "Dialogue", 3);
                break;
            }
            case InteractionElementType.CONDITION:
            {
                element.StateMachine = editorUtilities.ScriptableObjectField(element.StateMachine, "State Machine");

                if (element.StateMachine != null)
                {
                    element.Number1 = editorUtilities.IntPopupField(
                        element.Number1,
                        "Equal to",
                        element.StateMachine.States.ToArray(),
                        Enumerable.Range(0, element.StateMachine.States.Count + 1).ToArray());

                    element.Interaction = editorUtilities.ScriptableObjectField(element.Interaction, "Go to");
                }

                break;
            }
            case InteractionElementType.SET_STATE:
            {
                element.StateMachine = editorUtilities.ScriptableObjectField(element.StateMachine, "State Machine");

                if (element.StateMachine != null)
                {
                    element.Number1 = editorUtilities.IntPopupField(
                        element.Number1,
                        "Set to",
                        element.StateMachine.States.ToArray(),
                        Enumerable.Range(0, element.StateMachine.States.Count + 1).ToArray());
                }

                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
#region UTILITIES
    
    //todo: move this
    private class Utilities
        {
            private readonly float _startX;
            private float _x;
            private float _y;
            public Utilities(float x, float y)
            {
                _x = x;
                _startX = x;
                _y = y;
            }

            private const float LABEL_WIDTH = 100f;
            private const float FIELD_WIDTH = 300f;
            private const float TOP_MARGIN = 5f;
            private static readonly Color LINE_SEPARATOR_COLOR = new Color(1f, 1f, 1f, 0.2f);
            private const float LINE_SEPARATOR_THICKNESS = 1.5f;
            
            private static readonly float LINE_HEIGHT = EditorGUIUtility.singleLineHeight;

            private void LabelField(string label)
            {
                EditorGUI.LabelField(
                    new Rect(_x, _y, LABEL_WIDTH, LINE_HEIGHT),
                    label
                );
            }
            public void Label(string label)
            {
                LabelField(label);
                NewLine();
            }
            public Enum EnumField(Enum value, string label)
            {
                LabelField(label);

                value = EditorGUI.EnumPopup(
                    new Rect(_x + LABEL_WIDTH, _y, FIELD_WIDTH, LINE_HEIGHT),
                    value
                );
                
                NewLine();
                return value;
            }
            public Enum EnumFlagsField(Enum value, string label)
            {
                LabelField(label);

                value = EditorGUI.EnumFlagsField(
                    new Rect(_x + LABEL_WIDTH, _y, FIELD_WIDTH, LINE_HEIGHT),
                    value
                );
                
                NewLine();
                return value;
            }
            public int IntField(int value, string label)
            {
                LabelField(label);

                value = EditorGUI.IntField(
                    new Rect(_x + LABEL_WIDTH, _y, FIELD_WIDTH, LINE_HEIGHT),
                    value);
                
                NewLine();
                return value;
            }

            public float SliderField(float value, float min, float max, string label)
            {
                LabelField(label);

                value = EditorGUI.Slider(
                    new Rect(_x + LABEL_WIDTH, _y, FIELD_WIDTH, LINE_HEIGHT),
                    value, min, max);
                
                NewLine();
                return value;
            }
            
            // TODO: generic object field
            public T ScriptableObjectField<T>(T value, string label) where T : ScriptableObject
            {
                LabelField(label);
            
                value = (T) EditorGUI.ObjectField(
                    new Rect(_x + LABEL_WIDTH, _y, FIELD_WIDTH, LINE_HEIGHT),
                    value,
                    typeof(T),
                    false);
                
                NewLine();
                return value;
            }
            public string TextField(string value, string label, int lines = 1)
            {
                LabelField(label);

                value = EditorGUI.TextArea(
                    new Rect(_x + LABEL_WIDTH, _y, FIELD_WIDTH, lines * LINE_HEIGHT),
                    value,
                    EditorStyles.textArea);
                
                NewLine(lines);
                return value;
            }

            public bool BoolField(bool value, string label)
            {
                LabelField(label);
                
                value = EditorGUI.ToggleLeft(
                    new Rect(_x + LABEL_WIDTH, _y, 20f, LINE_HEIGHT),
                    string.Empty,
                    value);

                NewLine();
                return value;
            }

            // public T ObjectField<T> (T value, string label) where T : Object
            // {
            //     LabelField(label);
            //     
            //     value = (T) EditorGUI.ObjectField(
            //         new Rect(_x + LABEL_WIDTH, _y, FIELD_WIDTH, LINE_HEIGHT),
            //         value,
            //         typeof(T),
            //         false);
            //     
            //     NewLine();
            //     return value;
            // }

            public int IntPopupField(int value, string label, string[] names, int[] values)
            {
                LabelField(label);

                value = EditorGUI.IntPopup(
                    new Rect(_x + LABEL_WIDTH, _y, FIELD_WIDTH, LINE_HEIGHT),
                    value,
                    names,
                    values
                );
                
                NewLine();
                return value;
            }
            
            private void NewLine(int lines = 1)
            {
                _y += (LINE_HEIGHT) * lines;
                TopMargin();
                _x = _startX;
            }
            public void TopMargin()
            {
                _y += TOP_MARGIN;
            }
            public void DrawLineSeparator()
            {
                const float width = LABEL_WIDTH + FIELD_WIDTH;
                var line = new Rect(_x, _y, width, LINE_SEPARATOR_THICKNESS);
                EditorGUI.DrawRect(line, LINE_SEPARATOR_COLOR);
            }
        }
    
#endregion
}
