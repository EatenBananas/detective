using System;
using System.Threading;
using Cinemachine;
using InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Zenject;
using Camera = UnityEngine.Camera;

namespace CameraSystem.CameraRotationAroundPlayer
{
    public enum CameraMode
    {
        Lock,
        Free,
        FollowTarget,
    }
    
    public class CameraController : MonoBehaviour
    {
        #region Events
        public Action<CameraMode> OnCameraModeChange { get; set; }
        public Action<Transform> OnFollowTargetChange { get; set; }
        public Action<Transform> OnCameraLookAtTargetChange { get; set; }
        public Action<float> OnCameraSpeedChange { get; set; }
        
        #endregion
        
        #region Properties

        public CameraMode CameraMode
        {
            get => _cameraMode;
            set
            {
                if (_cameraMode == value) return;
                _cameraMode = value;
                
                switch (_cameraMode)
                {
                    case CameraMode.Lock:
                        break;
                    case CameraMode.Free:
                        CameraFollowTarget = null;
                        break;
                    case CameraMode.FollowTarget:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                OnCameraModeChange?.Invoke(_cameraMode);
            }
        }
        public Camera Camera => _camera;
        public float CameraSpeed
        {
            get => _cameraSpeed;
            set
            {
                if (Math.Abs(_cameraSpeed - value) < 0.001f) return;
                _cameraSpeed = value;
                OnCameraSpeedChange?.Invoke(_cameraSpeed);
            }
        }
        public Transform CameraFollowTarget
        {
            get => _cameraFollowTarget;
            set
            {
                if (_cameraFollowTarget == value) return;
                _cameraFollowTarget = value;
                _virtualCamera.Follow = _cameraFollowTarget;
                OnFollowTargetChange?.Invoke(_cameraFollowTarget);
            }
        }
        public Transform CameraLookAtTarget
        {
            get => _cameraLookAtTarget;
            set
            {
                if (_cameraLookAtTarget == value) return;
                _cameraLookAtTarget = value;
                _virtualCamera.LookAt = _cameraLookAtTarget;
                OnCameraLookAtTargetChange?.Invoke(_cameraLookAtTarget);
            }
        }

        #endregion
        
        #region Private Fields
        
        [SerializeField] private CameraMode _cameraMode;
        [SerializeField] private Camera _camera;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private CinemachineInputProvider _inputProvider;
        [SerializeField] private Transform _cameraFollowTarget;
        [SerializeField] private Transform _cameraLookAtTarget;
        [SerializeField] private float _cameraSpeed = 10;
        
        [Inject] private PlayerSystem.Player _player;
        [Inject] private InputManager _inputManager;
        
        private bool _cameraCanRotation;
        
        #endregion

        #region Unity Lifecycle
        
        private void OnEnable()
        {
            _inputManager.Input.Camera.Map.EnableInputActionMap();
        }
        private void OnDisable()
        {
            _inputManager.Input.Camera.Map.DisableInputActionMap();
        }
        private void Awake()
        {
            _inputManager.Input.Camera.Move.performed += SetCameraModeToFree;
            _inputManager.Input.Camera.Rotation.started += EnableCameraRotation;
            _inputManager.Input.Camera.Rotation.canceled += DisableCameraRotation;
            _inputManager.Input.Camera.FindPlayer.performed += FindPlayerHandler;
        }
        private void Update()
        {
            switch (CameraMode)
            {
                case CameraMode.Lock:
                    // TODO: Add camera lock mode
                    break;
                case CameraMode.Free:
                    CameraFreeModeMovement();
                    break;
                case CameraMode.FollowTarget:
                    CameraFollowTargetMovement(CameraFollowTarget);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        #endregion

        private void CameraFreeModeMovement()
        {
            if (CameraFollowTarget != null) CameraFollowTarget = null;
            
            // Get input
            var moveVector = _inputManager.Input.Camera.Move.ReadValue<Vector2>().normalized;
            
            // Get camera normalized direction
            var cameraTransform = Camera.transform;
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

        private void CameraFollowTargetMovement(Transform target)
        {
            CameraFollowTarget = target;
        }
        
        private void MoveCameraToPosition(Vector3 newCameraPosition)
        {
            transform.position = newCameraPosition;
        }

        #region Input Interactions

        private void SetCameraModeToFree(InputAction.CallbackContext context)
        {
            // CameraFollowTarget = _defCameraFollowTarget;
        }

        private void FindPlayerHandler(InputAction.CallbackContext context)
        {
            switch (context.interaction)
            {
                case TapInteraction:
                    // Only move camera to player
                    MoveCameraToPosition(_player.PlayerTransform.position);
                    break;
                case MultiTapInteraction:
                    // Bind camera to player
                    CameraFollowTarget = _player.PlayerTransform;
                    break;
            }
        }

        private void EnableCameraRotation(InputAction.CallbackContext context)
        {
            _inputProvider.enabled = true;
        }
        
        private void DisableCameraRotation(InputAction.CallbackContext context)
        {
            _inputProvider.enabled = false;
        }

        #endregion
    }
}
