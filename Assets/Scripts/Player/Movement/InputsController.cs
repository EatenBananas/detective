using CameraSystem;
using UnityEngine;
using UnityEngine.AI;
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
            if (_playerMovement.PlayerMovingState != PlayerMovingState.Sneaking)
                _playerMovement.SetPlayerMovingState(PlayerMovingState.Walking);
            
            if (GetMouseToWorldPosition(out var position)) 
                _playerMovement.SetPlayerTargetPosition(position);
        }

        private void OnSprint(InputAction.CallbackContext obj)
        {
            if (_playerMovement.PlayerMovingState != PlayerMovingState.Sneaking)
                _playerMovement.SetPlayerMovingState(PlayerMovingState.Sprinting);

            if (GetMouseToWorldPosition(out var position)) 
                _playerMovement.SetPlayerTargetPosition(position);
        }

        private static bool GetMouseToWorldPosition(out Vector3 position)
        {
            var mousePosition = PlayerInput.Player.MousePosition.ReadValue<Vector2>();
            var ray = CameraGetter.MainCamera.ScreenPointToRay(mousePosition);
            Physics.Raycast(ray, out var hit);

            if (NavMesh.SamplePosition(hit.point, out var navMeshHit, 0.1f,
                    NavMesh.GetAreaFromName(PlayerMovement.WalkableArea)))
            {
                position = navMeshHit.position;
                return true;
            }
            
            position = default;
            return false;
        }
    }
}
