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

// ReSharper disable once CheckNamespace
namespace EditorUtilities
{
    [CustomEditor(typeof(Interaction))]
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

    }
}