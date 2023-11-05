using UnityEngine;

namespace PlayerSystem
{
    public class Player : MonoBehaviour
    {
        public Transform PlayerTransform
        {
            get
            {
                if (_playerTransform == null) _playerTransform = transform;
                return _playerTransform;
            }
        }
        
        private Transform _playerTransform;
    }
}
