using System;
using Interactions.Elements;
using UnityEngine;

namespace UI
{
    [Serializable]
    public class PieMenuOption
    {
        [field:SerializeField] public PieMenuOptionType OptionType { get; set; }
        [field:SerializeField] public StartInteraction Interaction { get; set; }
    }
}
