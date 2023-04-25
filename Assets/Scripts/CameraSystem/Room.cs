using Player.Movement;
using UnityEngine;

namespace CameraSystem
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private Transform _cameraPosition;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.TryGetComponent(out PlayerMovement player))
                CameraPositioning.CameraPosition = _cameraPosition;
        }
    }
}
