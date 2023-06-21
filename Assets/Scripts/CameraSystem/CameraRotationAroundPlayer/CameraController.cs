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
        [SerializeField] private CinemachineInputProvider _inputProvider;
        [SerializeField] private GameObject CameraLookAtTarget;
        [SerializeField] private float _cameraSpeed = 10;

        private PlayerInputActions _inputActions;
        private CameraMode _cameraMode;
        private bool _cameraCanRotation;

        private void Awake()
        {
            _inputActions = new PlayerInputActions();

            _inputActions.Camera.Move.performed += _ => _cameraMode = CameraMode.Free;
            _inputActions.Camera.EnableRotation.started += EnableCameraRotation;
            _inputActions.Camera.EnableRotation.canceled += DisableCameraRotation;
        }

        private void OnEnable() => _inputActions.Enable();
        private void OnDisable() => _inputActions.Disable();

        private void Update()
        {
            if (_cameraMode == CameraMode.Free) CameraFreeMovement();
        }

        private void CameraFreeMovement()
        {
            // Get input
            var input = _inputActions.Camera.Move.ReadValue<Vector2>().normalized;
            
            // Get camera normalized direction
            var forward = _camera.transform.forward;
            var right = _camera.transform.right;
            forward.y = 0;
            right.y = 0;
            forward = forward.normalized;
            right = right.normalized;
            
            // Relative direction from input and camera
            var forwardRelative = input.y * forward;
            var rightRelative = input.x * right;
            
            // Camera movement
            var cameraMovement = (forwardRelative + rightRelative) * (_cameraSpeed * Time.deltaTime);
            transform.Translate(cameraMovement, Space.World);
        }

        private void EnableCameraRotation(InputAction.CallbackContext callbackContext)
        {
            _inputProvider.enabled = true;
        }

        private void DisableCameraRotation(InputAction.CallbackContext callbackContext)
        {
            _inputProvider.enabled = false;
        }
    }
}
