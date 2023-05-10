using System;
using Cinemachine;
using Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace CameraSystem.CameraRotationAroundPlayer
{
    public class CameraOffsetToPlayer : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;
        [SerializeField] private CinemachineVirtualCamera _virtualCam;
        [SerializeField] private Transform CameraRoot;
        
        
        
        [Range(0, 10)] [SerializeField] private float _mouseSensitivity = 1f;
        [Range(0, 1)] [SerializeField] private float _smoothness = 0.1f;
        [Range(0, 180)] [SerializeField] private float _cameraCeiling = 85f;
        [Range(0, 180)] [SerializeField] private float _cameraFlour = 25;


        private PlayerInputActions _playerInputActions;
        private Vector2 _rotation;
        private Vector3 _currentCamRotation;
        private Vector3 _smoothnessVelocity;
        

        private void Awake()
        {
            _playerInputActions = new PlayerInputActions();
            CameraRoot = transform.parent.transform;
        }

        private void OnEnable() => _playerInputActions.Enable();
        private void OnDisable() => _playerInputActions.Disable();

        private void OnValidate()
        {
            transform.position = _offset;
            CameraRoot.position = PlayerInfo.PlayerTransform.position;
        }

        private void Update()
        {
            UpdateCameraPosition();
            UpdateCameraPositionAndRotation();
        }

        private void UpdateCameraPosition()
        {
            var newCameraPosition = transform;
            newCameraPosition.position = PlayerInfo.PlayerTransform.position + _offset;
            _virtualCam.Follow = newCameraPosition;
        }
        
        private void UpdateCameraPositionAndRotation()
        {
            var mouseValue = _playerInputActions.Player.MousePosition.ReadValue<Vector2>() * _mouseSensitivity;
            mouseValue.y *= -1;

            _rotation += mouseValue;
            _rotation.y = Mathf.Clamp(_rotation.y, _cameraFlour, _cameraCeiling);

            var newCamRotation = new Vector3(_rotation.y, _rotation.x);


            _currentCamRotation = newCamRotation;
                // Vector3.SmoothDamp(_currentCamRotation, newCamRotation, ref _smoothnessVelocity, _smoothness);


            CameraRoot.localEulerAngles = _currentCamRotation;
        }

    }
}
