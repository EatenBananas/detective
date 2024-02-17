using System;
using GameInputSystem;
using UnityEngine;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

namespace PlayerSystem
{
    public class PlayerInput : MonoBehaviour
    {
        public event Action<RaycastHit> OnMove; 
        
        [Inject] private Player _player;
        [Inject] private InputManager _inputManager;
        [Inject] private Camera _camera;

        private void OnEnable()
        {
            _inputManager.Input.PlayerController.Move.performed += OnMovePerformed;
            EnableInput();
        }
        
        private void OnDisable()
        {
            _inputManager.Input.PlayerController.Move.performed -= OnMovePerformed;
            DisableInput();
        }
        
        private void OnMovePerformed(CallbackContext context)
        {
            var mousePosition = _inputManager.Input.Mouse.Position.ReadValue<Vector2>();
            var ray = _camera.ScreenPointToRay(mousePosition);

            if (!Physics.Raycast(ray, out var hit)) return;
            
            _player.Movement.SetMovementDestination(hit.point);
            
            OnMove?.Invoke(hit);
        }
        
        public void EnableInput()
        {
            _inputManager.Input.Mouse.Position.EnableInputAction();
            _inputManager.Input.PlayerController.Move.EnableInputAction();
        }

        public void DisableInput()
        {
            _inputManager.Input.Mouse.Position.DisableInputAction();
            _inputManager.Input.PlayerController.Move.DisableInputAction();
        }
    }
}