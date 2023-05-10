using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace CameraSystem.CameraLookingOnRoom
{
    public class CameraPositioning : MonoBehaviour
    {
        public static UnityEvent OnCameraChange = new();
        
        public static Transform CameraPosition
        {
            get => _cameraPosition;
            set
            {
                _cameraPosition = value;
                OnCameraChange.Invoke();
            }
        }

        private static Transform _cameraPosition;

        [SerializeField] private CinemachineVirtualCamera _virtualCam;

        private void Start()
        {
            OnCameraChange.AddListener(UpdateCameraPosition);
        }
        
        private void UpdateCameraPosition()
        {
            _virtualCam.Follow = _cameraPosition;
        }
    }
}
