using System;
using System.Collections.Generic;
using Equipment;
using Interactions.Elements;
using UnityEditor;
using UnityEngine;

namespace Interactions
{
    [Serializable]
    public class InteractionElementData
    {
        [field:SerializeField] public InteractionElementType Type { get; set; }
        [field:SerializeField] public KeyCode KeyCode { get; set; }
        [field:SerializeField] public int Number1 { get; set; }
        
        [field:SerializeField] public string Text1 { get; set; }
        [field:SerializeField] public string Text2 { get; set; }
        [field:SerializeField] public State StateMachine { get; set; }
        [field:SerializeField] public Interaction Interaction { get; set; }
        [field:SerializeField] public DialogueNpc DialogueNpc { get; set; }
        [field: SerializeField] public List<Option> Options { get; set; } = new List<Option>();
        [field: SerializeField] public Item Item { get; set; }

        public InteractionElement ToElement()
        {
            return Type switch
            {
                InteractionElementType.CHANGE_CAMERA => new CameraChange(this),
                InteractionElementType.GET_KEY => new GetKey(this),
                InteractionElementType.DIALOGUE => new Dialogue(this),
                InteractionElementType.CONDITION => new Condition(this),
                InteractionElementType.SET_STATE => new SetState(this),
                InteractionElementType.TELEPORT => new Teleport(this),
                InteractionElementType.CHOICE => new Choice(this),
                InteractionElementType.EQUIP => new Equip(this),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
#if UNITY_EDITOR        
        public float EditorHeight()
        {
            int lines = Type switch
            {
                InteractionElementType.CHANGE_CAMERA => 3,
                InteractionElementType.GET_KEY => 3,
                InteractionElementType.DIALOGUE => 6,
                InteractionElementType.CONDITION => 6,
                InteractionElementType.SET_STATE => 4,
                InteractionElementType.TELEPORT => 3,
                InteractionElementType.CHOICE => 3 + Options.Count * 4,
                InteractionElementType.EQUIP => 3,
                _ => throw new ArgumentOutOfRangeException()
            };

            return lines * EditorGUIUtility.singleLineHeight;
        }
#endif
    }
}