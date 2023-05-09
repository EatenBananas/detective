using System;
using UnityEngine;

namespace Interactions.Elements
{
    [Serializable]
    public class Option
    {
        [field:SerializeField] public string Text { get; set; }
        [field:SerializeField] public Interaction Outcome { get; set; }
    }
}
