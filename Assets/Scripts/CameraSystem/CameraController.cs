using System.Collections.Generic;
using Cinemachine;
using GameInputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Zenject;
using Camera = UnityEngine.Camera;

namespace CameraSystem
{
    public class CameraController : MonoBehaviour
    {
        #region Inspector Fields
        
        [SerializeField] private float _cameraSpeed = 10f;
        
        [Header("FreeMode")]
        [SerializeField] private CinemachineVirtualCamera _freeVCam;
        [SerializeField] private CinemachineInputProvider _freeVCamInputProvider;
        
        [Header("FollowTargetMode")]
        [SerializeField] private CinemachineVirtualCamera _followTargetVCam;

        #endregion
        
        #region Private Fields
        
        [Inject] private PlayerSystem.Player _player;
        [Inject] private InputManager _inputManager;
        [Inject] private Camera _camera;
        
        #endregion

        #region Unity Lifecycle
        
        private void OnEnable()
        {
            _inputManager.Input.CameraController.Map.EnableInputActionMap();
            
            _inputManager.Input.CameraController.Move.performed += OnPlayerMove;
            _inputManager.Input.CameraController.Rotation.started += OnPlayerStartRotatingCamera;
            _inputManager.Input.CameraController.Rotation.canceled += OnPlayerStopRotatingCamera;
            _inputManager.Input.CameraController.FindPlayer.performed += OnPlayerLookingForPlayer;
        }
        private void OnDisable()
        {
            _inputManager.Input.CameraController.Map.DisableInputActionMap();
            
            _inputManager.Input.CameraController.Move.performed -= OnPlayerMove;
            _inputManager.Input.CameraController.Rotation.started -= OnPlayerStartRotatingCamera;
            _inputManager.Input.CameraController.Rotation.canceled -= OnPlayerStopRotatingCamera;
            _inputManager.Input.CameraController.FindPlayer.performed -= OnPlayerLookingForPlayer;
        }
        private void Update()
        {
            if (_freeVCam.Priority >= 1)
                CameraFreeModeMovement();
        }
        
        #endregion
        
        #region Input Interactions

        private void OnPlayerMove(InputAction.CallbackContext context)
        {
            EnableVCam(_freeVCam);
        }
        private void OnPlayerLookingForPlayer(InputAction.CallbackContext context)
        {
            switch (context.interaction)
            {
                case TapInteraction:
                    // Only move camera to player
                    MoveCameraToPosition(_player.transform.position);
                    break;
                case MultiTapInteraction:
                    // Bind camera to player
                    var playerTransform = _player.transform;
                    FollowTarget(playerTransform, playerTransform);
                    break;
            }
        }
        private void OnPlayerStartRotatingCamera(InputAction.CallbackContext context)
        {
            _freeVCamInputProvider.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        private void OnPlayerStopRotatingCamera(InputAction.CallbackContext context)
        {
            _freeVCamInputProvider.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        #endregion
        
        private void FollowTarget(Transform followTarget, Transform lookAtTarget)
        {
            EnableVCam(_followTargetVCam);
            _followTargetVCam.Follow = followTarget;
            _followTargetVCam.LookAt = lookAtTarget;
        }
        private void EnableVCam(CinemachineVirtualCamera vCamToEnable)
        {
            var allVCams = new List<CinemachineVirtualCamera>
            {
                _freeVCam,
                _followTargetVCam
            };

            foreach (var virtualCamera in allVCams) 
                virtualCamera.Priority = virtualCamera == vCamToEnable ? 1 : 0;
        }

        private void CameraFreeModeMovement()
        {
            // Get input
            var moveVector = _inputManager.Input.CameraController.Move.ReadValue<Vector2>().normalized;
            
            // Get camera normalized direction
            var cameraTransform = _camera.transform;
            var forward = cameraTransform.forward;
            var right = cameraTransform.right;
            forward.y = 0;
            right.y = 0;
            forward = forward.normalized;
            right = right.normalized;
            
            // Relative direction from input and camera
            var forwardRelative = moveVector.y * forward;
            var rightRelative = moveVector.x * right;
            
            // Camera movement
            var cameraMovement = (forwardRelative + rightRelative) * (_cameraSpeed * Time.deltaTime);
            transform.Translate(cameraMovement, Space.World);
        }
        private void MoveCameraToPosition(Vector3 newCameraPosition)
        {
            transform.position = newCameraPosition;
        }
    }
}
