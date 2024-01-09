using UnityEngine;

namespace InfoSystem
{
    public class GenericInfo : MonoBehaviour, IInfo
    {
        public string Title;
        public string Description;
        public Sprite Icon;
        
        public string GetTitle() => Title;
        public string GetDescription() => Description;
        public Sprite GetIcon() => Icon;
    }
}