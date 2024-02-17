using UnityEngine;

namespace ToolTipSystem
{
    public class GenericToolTip : MonoBehaviour, IToolTip
    {
        [field: SerializeField] public ToolTipData ToolTipData { get; set; }
    }
}