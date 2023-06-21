using System;
using SceneObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Interactions.Elements
{
    [Serializable]
    public class Teleport : InteractionElement
    {
        [field: SerializeField] public SceneReference Location { get; set; }

        public override void Execute()
        {
            SceneObjectManager.Instance.Teleport(Location);
            InteractionManager.Instance.CompleteElement();
        }
        
#if UNITY_EDITOR
        public override int Height() => 3;
#endif
    }
}
