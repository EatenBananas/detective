using System;
using UnityEngine;

namespace Interactions.Elements
{
    [Serializable]
    public class CameraChange : InteractionElement
    {
        [field: SerializeField] public int CameraId { get; set; } = 0;
        public override void Execute()
        {
            CameraManager.Instance.ChangeCamera(CameraId);
            InteractionManager.Instance.CompleteElement();
        }
        
#if UNITY_EDITOR
        public override int Height() => 3;
#endif

    }
}