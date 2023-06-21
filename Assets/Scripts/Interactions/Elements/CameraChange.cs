using System;
using SceneObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Interactions.Elements
{
    [Serializable]
    public class CameraChange : InteractionElement
    {
        [field: SerializeField] public SceneReference Camera { get; set; }
        public override void Execute()
        {
            CameraManager.Instance.ChangeCamera(Camera);
            InteractionManager.Instance.CompleteElement();
        }
        
#if UNITY_EDITOR
        public override int Height() => 3;
#endif

    }
}