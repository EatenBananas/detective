using CameraSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Movement
{
    public class InputsController : MonoBehaviour
    {
        public static PlayerInputActions PlayerInput;
        
        private PlayerMovement _playerMovement;

        private void OnEnable() => PlayerInput.Enable();
        private void OnDisable() => PlayerInput.Disable();

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            PlayerInput = new PlayerInputActions();

            PlayerInput.Player.Sneak.started += OnStartedSneak;
            PlayerInput.Player.Sneak.canceled += OnCanceledSneak;
            PlayerInput.Player.Walk.performed += OnWalk;
            PlayerInput.Player.Sprint.performed += OnSprint;
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

        private void OnWalk(InputAction.CallbackContext obj)
        {
            if (PlayerMovement.PlayerMovingState != PlayerMovingState.Sneaking)
                _playerMovement.SetPlayerMovingState(PlayerMovingState.Walking);
            
            _playerMovement.SetPlayerTargetPosition(GetMouseToWorldPosition());
        }

        private void OnSprint(InputAction.CallbackContext obj)
        {
            if (PlayerMovement.PlayerMovingState != PlayerMovingState.Sneaking)
                _playerMovement.SetPlayerMovingState(PlayerMovingState.Sprinting);
            
            _playerMovement.SetPlayerTargetPosition(GetMouseToWorldPosition());
        }

        private static Vector3 GetMouseToWorldPosition()
        {
            var mousePosition = PlayerInput.Player.MousePosition.ReadValue<Vector2>();
            var ray = CameraGetter.MainCamera.ScreenPointToRay(mousePosition);
            Physics.Raycast(ray, out var hit);
            return hit.point;
        }
    }
}
