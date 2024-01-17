#if UNITY_EDITOR
using System;
using UnityEngine;

namespace GraphEditor.Saves
{
    [Serializable]
    public class OutcomeSave
    {
        [field: SerializeField] public int Value { get; set; } = 1;
        [field: SerializeField] public string NodeID { get; set; } = string.Empty;
    }
}
#endif