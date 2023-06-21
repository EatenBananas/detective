using UnityEngine;

namespace CameraSystem
{
    public class CameraGetter : MonoBehaviour
    {
        public static Camera MainCamera
        {
            get
            {
                if (_mainCamera == null) _mainCamera = Camera.main;
                return _mainCamera;
            }
        }

        private static Camera _mainCamera;
    }
}
