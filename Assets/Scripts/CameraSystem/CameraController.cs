using System;
using System.Collections.Generic;
using Cinemachine;
using GameInputSystem;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Zenject;
using Camera = UnityEngine.Camera;

namespace CameraSystem
{
    public enum CameraMode
    {
        Free,
        FollowTarget
    }
    
    public class CameraController : MonoBehaviour
    {
        public CameraMode CameraMode
        {
            get => _cameraMode;
            set => _cameraMode = value;
        }

        public Transform FollowTarget
        {
            get => _followTarget;
            set
            {
                _followTarget = value;
                _vCam.Follow = _followTarget;
            }
        }
        
        public Transform LookAtTarget
        {
            get => _lookAtTarget;
            set
            {
                _lookAtTarget = value;
                _vCam.LookAt = _lookAtTarget;
            }
        }
        
        #region Inspector Fields
     
        [SerializeField] private CameraMode _cameraMode;
        [SerializeField] private Transform _followTarget;
        [SerializeField] private Transform _lookAtTarget;
        [SerializeField] private float _cameraSpeed = 10f;
        [SerializeField] private bool _followPlayerOnStart;
        
        [Header("Cinemachine")]
        [SerializeField] private CinemachineVirtualCamera _vCam;
        [SerializeField] private CinemachineInputProvider _freeVCamInputProvider;
        [SerializeField] private Transform _defFollowTarget;
        [SerializeField] private Transform _defLookAtTarget;

        #endregion
        
        #region Private Fields
        
        [Inject] private PlayerSystem.Player _player;
        [Inject] private InputManager _inputManager;
        [Inject] private Camera _camera;
        
        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            switch (CameraMode)
            {
                case CameraMode.Free:
                    FollowTarget = _defFollowTarget;
                    LookAtTarget = _defLookAtTarget;
                    break;
                case CameraMode.FollowTarget:
                    FollowTarget = _followTarget;
                    LookAtTarget = _lookAtTarget;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            if (_followPlayerOnStart)
            {
                CameraMode = CameraMode.FollowTarget;
                var playerTransform = _player.transform;
                FollowTarget = playerTransform;
                LookAtTarget = playerTransform;
                MoveCameraToPosition(playerTransform.position);
            }
        }

        private void OnEnable()
        {
            _inputManager.Input.CameraController.Map.EnableInputActionMap();
            
            _inputManager.Input.CameraController.Move.performed += OnMove;
            _inputManager.Input.CameraController.Rotation.started += OnRotationStart;
            _inputManager.Input.CameraController.Rotation.canceled += OnRotationCancel;
            _inputManager.Input.CameraController.FindPlayer.performed += OnFindPlayer;
            _inputManager.Input.CameraController.Zoom.performed += OnZoom;
            
            ResetFieldOfView();
        }
        
        private void OnDisable()
        {
            _inputManager.Input.CameraController.Map.DisableInputActionMap();
            
            _inputManager.Input.CameraController.Move.performed -= OnMove;
            _inputManager.Input.CameraController.Rotation.started -= OnRotationStart;
            _inputManager.Input.CameraController.Rotation.canceled -= OnRotationCancel;
            _inputManager.Input.CameraController.FindPlayer.performed -= OnFindPlayer;
            _inputManager.Input.CameraController.Zoom.performed -= OnZoom;
            
            ResetFieldOfView();
        }
        
        private void Update()
        {
            switch (CameraMode)
            {
                case CameraMode.Free:
                    CameraFreeModeMovement();
                    break;
                case CameraMode.FollowTarget:
                    _defFollowTarget.position = FollowTarget.position;
                    _defLookAtTarget.position = LookAtTarget.position;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        #endregion
        
        #region Input Interactions

        private void OnMove(InputAction.CallbackContext context)
        {
            FollowTarget = _defFollowTarget;
            LookAtTarget = _defLookAtTarget;
            
            CameraMode = CameraMode.Free;
        }
        
        private void OnFindPlayer(InputAction.CallbackContext context)
        {
            switch (context.interaction)
            {
                case TapInteraction:
                    // Only move camera to player
                    CameraMode = CameraMode.Free;
                    FollowTarget = _defFollowTarget;
                    LookAtTarget = _defLookAtTarget;
                    MoveCameraToPosition(_player.transform.position);
                    break;
                case MultiTapInteraction:
                    // Bind camera to player
                    CameraMode = CameraMode.FollowTarget;
                    var playerTransform = _player.transform;
                    FollowTarget = playerTransform;
                    LookAtTarget = playerTransform;
                    MoveCameraToPosition(playerTransform.position);
                    break;
            }
        }
        
        private void OnRotationStart(InputAction.CallbackContext context)
        {
            _freeVCamInputProvider.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        private void OnRotationCancel(InputAction.CallbackContext context)
        {
            _freeVCamInputProvider.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        #endregion

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
            
            
            var moveDirection = (forwardRelative + rightRelative) * (_cameraSpeed * Time.deltaTime);
            
            var newCamPos = transform.position + moveDirection;
            
            NavMesh.SamplePosition(newCamPos, out var hit, 1f, NavMesh.AllAreas);
            
            MoveCameraToPosition(hit.position);
        }
        
        private void MoveCameraToPosition(Vector3 newCameraPosition)
        {
            transform.position = newCameraPosition;
        }

        private void OnZoom(InputAction.CallbackContext context)
        {
            var zoomValue = context.ReadValue<Vector2>().y;
            
            var isZoomUp = zoomValue > 0;
            var isZoomDown = zoomValue < 0;

            if (isZoomUp)
                _vCam.m_Lens.FieldOfView = Mathf.Clamp(_vCam.m_Lens.FieldOfView - 5, 30, 100);
            else if (isZoomDown)
                _vCam.m_Lens.FieldOfView = Mathf.Clamp(_vCam.m_Lens.FieldOfView + 5, 30, 100);
        }
        
        private void ResetFieldOfView()
        {
            _vCam.m_Lens.FieldOfView = 60;
        }
    }
}
