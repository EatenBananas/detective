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
                elementHeightCallback = ElementHeight,
                
                displayAdd = true,
                onAddCallback = list =>  ShowAddElementDropdown(),
                displayRemove = true
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawInteractionGUI();
            EditorUtility.SetDirty(_interaction);
            serializedObject.ApplyModifiedProperties();
        }

        private void ShowAddElementDropdown()
        {
            GenericMenu genericMenu = new();
            foreach (var subType in InteractionElement.SubTypes)
            {
                genericMenu.AddItem(new GUIContent(subType.Name), false,
                    () => _interaction.Elements.Add((InteractionElement) Activator.CreateInstance(subType)));
            }
            genericMenu.ShowAsContext();
        }
        
        private void DrawInteractionGUI()
        {
            _reorderableList.DoLayoutList();
        }

        private void DrawListItem(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = _reorderableList.list[index] as InteractionElement;
            Debug.Assert(element != null, "element != null");
            var utils = new Utilities(rect.x, rect.y);
        
            //utils.TopMargin();

            //element.Type = (InteractionElementType) utils.EnumField(element.Type, "Type");
            //utils.Label(element.GetType().Name);
            element.Folded = utils.FoldoutField(element.Folded, element.GetType().Name);

            if (element.Folded)
            {
                return;
            }
            
            // temp
            switch (element)
            {
                case CameraChange cameraChange:
                {
                    //Debug.Assert(CameraManager.Instance != null, "CameraManager.Instance != null");
                    //temp find
        
                    var cm = FindObjectOfType<CameraManager>();
        
                    //element.Number1 = editorUtilities.IntField(element.Number1, "Camera ID");
                    cameraChange.CameraId = utils.IntPopupField(cameraChange.CameraId, "Camera",
                        //new[] {"Main", "Mailbox", "Receptionist"}, new[] {0, 1, 2});
                        cm.GetCameraNames(),
                        cm.GetCameraIndexes());
                    break;
                }
                case GetKey getKey:
                {
                    getKey.Key = (KeyCode) utils.EnumField(getKey.Key, "Key");
                    break;
                }
                case Dialogue dialogue:
                {
                    dialogue.DialogueNpc = utils.ScriptableObjectField(dialogue.DialogueNpc, "NPC");
                    dialogue.DialogueText = utils.TextField(dialogue.DialogueText, "Dialogue", 3);
                    break;
                }
                case Condition condition:
                {
                    condition.StateMachine = utils.ScriptableObjectField(condition.StateMachine, "State Machine");
        
                    if (condition.StateMachine != null)
                    {
                        condition.EqualTo = utils.IntPopupField(
                            condition.EqualTo,
                            "Equal to",
                            condition.StateMachine.States.ToArray(),
                            Enumerable.Range(0, condition.StateMachine.States.Count + 1).ToArray());
        
                        condition.GoTo = utils.ScriptableObjectField(condition.GoTo, "Go to");
                    }
        
                    break;
                }
                case SetState setState:
                {
                    setState.StateMachine = utils.ScriptableObjectField(setState.StateMachine, "State Machine");
        
                    if (setState.StateMachine != null)
                    {
                        setState.State = utils.IntPopupField(
                            setState.State,
                            "Set to",
                            setState.StateMachine.States.ToArray(),
                            Enumerable.Range(0, setState.StateMachine.States.Count + 1).ToArray());
                    }
        
                    break;
                }
                // case InteractionElementType.TELEPORT:
                // {
                //     //temp find
                //     var om = FindObjectOfType<ObjectManager>();
                //     
                //     element.Number1 = utils.IntPopupField(
                //         element.Number1,
                //         "Location",
                //         om.GetObjectNames(),
                //         om.GetObjectIndexes());
                //     
                //     break;
                // }
        
                case Choice choice:
                {
                    var options = choice.Options;
                    var desiredCount = Math.Max(utils.IntField(options.Count, "Count"), 0);
        
                    while (desiredCount > options.Count)
                    {
                        choice.Options.Add(new Option());
                    }
        
                    while (desiredCount < options.Count && options.Count > 0)
                    {
                        choice.Options.RemoveAt(choice.Options.Count-1);
                    }
                    
                    foreach (var option in choice.Options)
                    {
                        utils.Label("Option");
                        option.Text = utils.TextField(option.Text, "Name");
                        option.Outcome = utils.ScriptableObjectField(option.Outcome, "Outcome");
                    }
                    break;
                }
                case Equip equip:
                {
                    equip.Item = utils.ScriptableObjectField(equip.Item, "Item");
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private float ElementHeight(int index)
        {
            var element = _interaction.Elements[index];

            if (element.Folded)
            {
                return EditorGUIUtility.singleLineHeight;
            }
            
            int height = element.Height();
            return height * EditorGUIUtility.singleLineHeight;
        }
    }
}