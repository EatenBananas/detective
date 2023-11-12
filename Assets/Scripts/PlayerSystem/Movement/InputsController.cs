using CameraSystem.CameraRotationAroundPlayer;
using InputSystem;
using Player.Movement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Zenject;

namespace PlayerSystem.Movement
{
    public class InputsController : MonoBehaviour
    {
        [Inject] private CameraController _cameraController;
        [Inject] private InputManager _inputManager;
        
        private PlayerMovement _playerMovement;
        private bool _isMouseOverUI;

        private void OnEnable() => _inputManager.Input.Player.Map.EnableInputActionMap();
        private void OnDisable() => _inputManager.Input.Player.Map.DisableInputActionMap();

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();

            _inputManager.Input.Player.Move.performed += MoveHandler;
            _inputManager.Input.Player.Sneak.started += OnStartedSneak;
            _inputManager.Input.Player.Sneak.canceled += OnCanceledSneak;
        }

        private void Update()
        {
            IsMouseOverUI();
        }

        private void MoveHandler(InputAction.CallbackContext context)
        {
            if (context.interaction is MultiTapInteraction)
            {
                Sprint();
                return;
            }
            
            Walk();
        }

        private void OnCanceledSneak(InputAction.CallbackContext obj)
        {
            Debug.Log($"OnCanceledSneak");
            _playerMovement.SetPlayerMovingState(PlayerMovingState.Walking);
        }

        private void OnStartedSneak(InputAction.CallbackContext obj)
        {
            Debug.Log($"OnStartedSneak");
            _playerMovement.SetPlayerMovingState(PlayerMovingState.Sneaking);
        }

        private void Walk()
        {
            if (_playerMovement.PlayerMovingState != PlayerMovingState.Sneaking)
                _playerMovement.SetPlayerMovingState(PlayerMovingState.Walking);
            
            if (GetMouseToWorldPosition(out var position)) 
                _playerMovement.SetPlayerTargetPosition(position);
        }

        private void Sprint()
        {
            if (_playerMovement.PlayerMovingState != PlayerMovingState.Sneaking)
                _playerMovement.SetPlayerMovingState(PlayerMovingState.Sprinting);

            if (GetMouseToWorldPosition(out var position)) 
                _playerMovement.SetPlayerTargetPosition(position);
        }

        private bool GetMouseToWorldPosition(out Vector3 position)
        {
            var mousePosition = _inputManager.Input.Mouse.Position.ReadValue<Vector2>();
            var ray = _cameraController.Camera.ScreenPointToRay(mousePosition);
            Physics.Raycast(ray, out var hit);

            if (_isMouseOverUI)
            {
                position = default;
                return false;
            }

            if (NavMesh.SamplePosition(hit.point, out var navMeshHit, 1f,
                    NavMesh.AllAreas))
            {
                position = navMeshHit.position;
                return true;
            }

            position = default;
            return false;
        }

        private void IsMouseOverUI()
        {
            if (EventSystem.current != null) 
                _isMouseOverUI = EventSystem.current.IsPointerOverGameObject();
        }
    }
}
