using System;
using UnityEngine;

namespace Interactions.Elements
{
    [Serializable]
    public class CameraChange : InteractionElement
    {
        [field:SerializeField] public int CameraId { get; set; }
        public override void Execute()
        {
            CameraManager.Instance.ChangeCamera(CameraId);
            InteractionManager.Instance.CompleteElement();
        }
    }
}