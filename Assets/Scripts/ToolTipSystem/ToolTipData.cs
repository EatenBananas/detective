using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ToolTipSystem
{
    [Serializable]
    public struct ToolTipData
    {
        public ToolTipType Type;
        public string Title;
        public string Description;
        public Sprite Icon;
    }
}