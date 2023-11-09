using System;
using UnityEngine;

namespace GraphEditor
{
    [Serializable]
    public class Choice
    {
        [field: SerializeField] public string Text { get; set; } = string.Empty;
        [field: SerializeField] public string NodeID { get; set; } = string.Empty;
    }
}