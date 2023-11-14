using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "New Option")]
    public class PieMenuOptionType : ScriptableObject
    {
        [field: SerializeField] public string DisplayText { get; set; }
        [field: SerializeField] public Sprite Icon { get; set; }
    }
}