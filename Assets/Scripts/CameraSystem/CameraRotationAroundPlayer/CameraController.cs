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
        Free,
        Lock
    }
    
    public class CameraController : MonoBehaviour
    {
        public Transform FollowTarget;
        public Camera Camera => _camera;
        
        [SerializeField] private Camera _camera;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private CinemachineInputProvider _inputProvider;
        [SerializeField] private GameObject _cameraLookAtTarget;
        [SerializeField] private float _cameraSpeed = 10;

        [Inject] private PlayerSystem.Player _player;
        [Inject] private InputManager _inputManager;
        
        private CameraMode _cameraMode;
        private bool _cameraCanRotation;

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
            _inputManager.Input.Camera.Move.performed += FreeModeCamera;
            _inputManager.Input.Camera.Rotation.started += EnableCameraRotation;
            _inputManager.Input.Camera.Rotation.canceled += DisableCameraRotation;
            _inputManager.Input.Camera.FindPlayer.performed += FindPlayerHandler;
        }

        private void Update()
        {
            if (FollowTarget != null)
            {
                _cameraMode = CameraMode.Lock;
                transform.position = FollowTarget.position;
            }
            
            if (_cameraMode == CameraMode.Free) CameraFreeMovement();
        }
        
        #endregion
        
        private void FindPlayerHandler(InputAction.CallbackContext context)
        {
            switch (context.interaction)
            {
                case TapInteraction:
                    // Move camera to player
                    MoveCameraToPosition(_player.PlayerTransform.position);
                    break;
                case MultiTapInteraction:
                    // Bind camera to player
                    FollowTarget = _player.PlayerTransform;
                    break;
            }
        }

        private void FreeModeCamera(InputAction.CallbackContext obj)
        {
            FollowTarget = null;
            _cameraMode = CameraMode.Free;
        }

        private void CameraFreeMovement()
        {
            // Get input
            var input = _inputManager.Input.Camera.Move.ReadValue<Vector2>().normalized;
            
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

        public void MoveCameraToPosition(Vector3 newCameraPosition)
        {
            transform.position = newCameraPosition;
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
