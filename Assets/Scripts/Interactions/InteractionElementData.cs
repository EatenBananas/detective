using System;
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
        [field:SerializeField] public StateMachine StateMachine { get; set; }
        [field:SerializeField] public Interaction Interaction { get; set; }
    }
}