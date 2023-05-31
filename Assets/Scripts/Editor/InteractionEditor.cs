using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interactions;
using Interactions.Elements;
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
                drawElementCallback = DrawListItem,
                elementHeightCallback = ElementHeight
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
            var utils = new Utilities(rect.x, rect.y);

            utils.TopMargin();

            element.Type = (InteractionElementType) utils.EnumField(element.Type, "Type");

            // temp
            switch (element.Type)
            {
                case InteractionElementType.CHANGE_CAMERA:
                {
                    //Debug.Assert(CameraManager.Instance != null, "CameraManager.Instance != null");
                    //temp find

                    var cm = FindObjectOfType<CameraManager>();

                    //element.Number1 = editorUtilities.IntField(element.Number1, "Camera ID");
                    element.Number1 = utils.IntPopupField(element.Number1, "Camera",
                        //new[] {"Main", "Mailbox", "Receptionist"}, new[] {0, 1, 2});
                        cm.GetCameraNames(),
                        cm.GetCameraIndexes());
                    break;
                }
                case InteractionElementType.GET_KEY:
                {
                    element.KeyCode = (KeyCode) utils.EnumField(element.KeyCode, "Key");
                    break;
                }
                case InteractionElementType.DIALOGUE:
                {
                    element.DialogueNpc = utils.ScriptableObjectField(element.DialogueNpc, "NPC");
                    element.Text2 = utils.TextField(element.Text2, "Dialogue", 3);
                    break;
                }
                case InteractionElementType.CONDITION:
                {
                    element.StateMachine = utils.ScriptableObjectField(element.StateMachine, "State Machine");

                    if (element.StateMachine != null)
                    {
                        element.Number1 = utils.IntPopupField(
                            element.Number1,
                            "Equal to",
                            element.StateMachine.States.ToArray(),
                            Enumerable.Range(0, element.StateMachine.States.Count + 1).ToArray());

                        element.Interaction = utils.ScriptableObjectField(element.Interaction, "Go to");
                    }

                    break;
                }
                case InteractionElementType.SET_STATE:
                {
                    element.StateMachine = utils.ScriptableObjectField(element.StateMachine, "State Machine");

                    if (element.StateMachine != null)
                    {
                        element.Number1 = utils.IntPopupField(
                            element.Number1,
                            "Set to",
                            element.StateMachine.States.ToArray(),
                            Enumerable.Range(0, element.StateMachine.States.Count + 1).ToArray());
                    }

                    break;
                }
                case InteractionElementType.TELEPORT:
                {
                    //temp find
                    var om = FindObjectOfType<ObjectManager>();
                    
                    element.Number1 = utils.IntPopupField(
                        element.Number1,
                        "Location",
                        om.GetObjectNames(),
                        om.GetObjectIndexes());
                    
                    break;
                }

                case InteractionElementType.CHOICE:
                {
                    var options = element.Options;
                    var desiredCount = Math.Max(utils.IntField(options.Count, "Count"), 0);

                    while (desiredCount > options.Count)
                    {
                        element.Options.Add(new Option());
                    }

                    while (desiredCount < options.Count && options.Count > 0)
                    {
                        element.Options.RemoveAt(element.Options.Count-1);
                    }
                    
                    foreach (var option in element.Options)
                    {
                        utils.Label("Option");
                        option.Text = utils.TextField(option.Text, "Name");
                        option.Outcome = utils.ScriptableObjectField(option.Outcome, "Outcome");
                    }
                    break;
                }
                case InteractionElementType.EQUIP:
                {
                    element.Item = utils.ScriptableObjectField(element.Item, "Item");
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private float ElementHeight(int index)
        {
            return _interaction.Elements[index].EditorHeight();
        }
    }
}