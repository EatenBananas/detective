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
        public Transform FollowTarget;
        
        [SerializeField] private Camera _camera;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private CinemachineInputProvider _inputProvider;
        [SerializeField] private GameObject _cameraLookAtTarget;
        [SerializeField] private float _cameraSpeed = 10;

        private PlayerInputActions _inputActions;
        private CameraMode _cameraMode;
        private bool _cameraCanRotation;

        private void OnEnable() => _inputActions.Enable();
        private void OnDisable() => _inputActions.Disable();
        
        private void Awake()
        {
            _inputActions = new PlayerInputActions();

            _inputActions.Camera.Move.performed += FreeModeCamera;
            _inputActions.Camera.EnableRotation.started += EnableCameraRotation;
            _inputActions.Camera.EnableRotation.canceled += DisableCameraRotation;
            _inputActions.Camera.FindPlayer.performed += FindPlayer;
            _inputActions.Camera.BondToPlayer.performed += BindCameraToPlayer;
        }

        private void FreeModeCamera(InputAction.CallbackContext obj)
        {
            FollowTarget = null;
            _cameraMode = CameraMode.Free;
        }

        private void Update()
        {
            if (FollowTarget != null)
            {
                if (_cameraMode != CameraMode.Lock) _cameraMode = CameraMode.Lock;
                transform.position = FollowTarget.position;
            }
            
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

        public void MoveCameraToPosition(Vector3 newCameraPosition)
        {
            transform.position = newCameraPosition;
        }

        private void FindPlayer(InputAction.CallbackContext callbackContext)
        {
            if (Getter.Instance.Player != null) MoveCameraToPosition(Getter.Instance.Player.position);
        }

        private void EnableCameraRotation(InputAction.CallbackContext callbackContext)
        {
            _inputProvider.enabled = true;
        }

        private void DisableCameraRotation(InputAction.CallbackContext callbackContext)
        {
            _inputProvider.enabled = false;
        }

        private void BindCameraToPlayer(InputAction.CallbackContext obj)
        {
            FollowTarget = Getter.Instance.Player;
        }
    }
}
