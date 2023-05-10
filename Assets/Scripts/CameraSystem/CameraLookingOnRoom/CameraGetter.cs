using UnityEngine;

namespace CameraSystem
{
    public class CameraGetter : MonoBehaviour
    {
        public static Camera MainCamera;

        private void Awake()
        {
            MainCamera = GetComponent<Camera>();
        }
    }
}
