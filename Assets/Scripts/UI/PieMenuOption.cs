using System;
using UnityEngine;

namespace UI
{
    [Serializable]
    public class PieMenuOption
    {
        [field:SerializeField] public string DisplayText { get; set; }
        [field:SerializeField] public Sprite Icon { get; set; }
    }
}
