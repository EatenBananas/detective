using System;
using System.Collections.Generic;
using Equipment;
using Interactions.Elements;
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
    }
}