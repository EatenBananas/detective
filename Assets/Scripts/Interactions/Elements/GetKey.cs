using System;
using UnityEngine;

namespace Interactions.Elements
{
    [Serializable]
    public class GetKey : InteractionElement
    {
        [field: SerializeField] public KeyCode Key { get; set; } 

        public override void Execute()
        {
            InteractionManager.Instance.ListenForKey(Key);
        }
        
#if UNITY_EDITOR
        public override int Height() => 3;
#endif
    }
}