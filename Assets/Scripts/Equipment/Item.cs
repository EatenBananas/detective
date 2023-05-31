using UnityEngine;

namespace Equipment
{
    [CreateAssetMenu(fileName = "New Item")]
    public class Item : ScriptableObject
    {
        // avoiding name conflict with object.name
        [field:SerializeField] public string ItemName { get; set; }
        [field:SerializeField] public Sprite Icon { get; set; }
        
    }
}
