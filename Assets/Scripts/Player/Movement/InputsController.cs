using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Movement
{
    public class InputsController : MonoBehaviour
    {
        private PlayerInputActions _playerInput;
        private PlayerMovement _playerMovement;

        private void OnEnable()
        {
            _playerInput.Enable();
        }

        private void OnDisable()
        {
            _playerInput.Disable();
        }

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            
            _playerInput = new PlayerInputActions();

            _playerInput.Player.LeftClick.canceled += OnLeftClick;
        }

        private void OnLeftClick(InputAction.CallbackContext obj)
        {
            // Get mouse pos
            var mousePosition = _playerInput.Player.MousePosition.ReadValue<Vector2>();
            
            if (Camera.main != null)
            {
                var ray = Camera.main.ScreenPointToRay(mousePosition);
                if (!Physics.Raycast(ray, out var hit)) return;
                
                // Move Player to hit point from mouse ray
                _playerMovement.SetPlayerTargetPosition(hit.point);
            }
        }
    }
}
