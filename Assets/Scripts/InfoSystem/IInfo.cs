using UnityEngine;

namespace InfoSystem
{
    public interface IInfo
    {
        public string GetTitle();
        public string GetDescription();
        public Sprite GetIcon();
    }
}