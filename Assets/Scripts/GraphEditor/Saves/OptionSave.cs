using System;
using UnityEngine;

namespace GraphEditor.Saves
{
    [Serializable]
    public class OptionSave
    {
        [field: SerializeField] public string Text { get; set; } = string.Empty;
        [field: SerializeField] public string NodeID { get; set; } = string.Empty;
    }
}