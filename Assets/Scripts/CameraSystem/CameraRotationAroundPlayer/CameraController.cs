using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CameraSystem.CameraRotationAroundPlayer
{
    public enum CameraMode
    {
        Free,
        Lock
    }
    
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private GameObject CameraLookAtTarget;
        [SerializeField] private float _cameraSpeed;

        private PlayerInputActions _inputActions;

        private void Awake()
        {
            _inputActions = new PlayerInputActions();
            
            _inputActions.Camera.Move.performed += CameraMove;
        }

        private void OnEnable() => _inputActions.Enable();
        private void OnDisable() => _inputActions.Disable();
        
        private void CameraMove(InputAction.CallbackContext input)
        {
            var direction = input.ReadValue<Vector2>();

            var cameraRootPosition = transform.position;
            cameraRootPosition.x += direction.x * _cameraSpeed * Time.deltaTime;
            cameraRootPosition.z += direction.y * _cameraSpeed * Time.deltaTime;
            transform.position = cameraRootPosition;
        }

        private void CameraRotate()
        {
            
        }
    }
}
