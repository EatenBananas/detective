using UnityEngine;

namespace Interactions.Elements
{
    public class CameraChange : InteractionElement
    {
        private int _cameraId;
        
        public CameraChange(InteractionElementData data)
        {
            _cameraId = data.Number1;
        }

        public override void Execute()
        {
            CameraManager.Instance.ChangeCamera(_cameraId);
            InteractionManager.Instance.CompleteElement();
            UIManager.Instance.HideEquipment();
        }
    }
}