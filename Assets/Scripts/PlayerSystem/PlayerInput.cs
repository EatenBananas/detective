using System;
using GameInputSystem;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Interactions;
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
            _inputManager.Input.PlayerController.Sneak.performed += OnSneak;
            _inputManager.Input.PlayerController.Walk.performed += OnWalk;
            _inputManager.Input.PlayerController.Sprint.performed += OnSprint;
            
            EnableInput();
        }
        
        private void OnDisable()
        {
            _inputManager.Input.PlayerController.Sneak.performed -= OnSneak;
            _inputManager.Input.PlayerController.Walk.performed -= OnWalk;
            _inputManager.Input.PlayerController.Sprint.performed -= OnSprint;
            
            DisableInput();
        }

        private void OnSneak(CallbackContext context)
        {
            _player.Movement.IsCrouching = !_player.Movement.IsCrouching;
            _player.Movement.IsWalking = !_player.Movement.IsCrouching;
            _player.Movement.IsRunning = false;
        }

        private void OnWalk(CallbackContext context)
        {
            if (_player.Movement.IsCrouching)
            {
                PerformLocomotion();
                return;
            }
            
            _player.Movement.IsWalking = true;
            _player.Movement.IsRunning = false;
            
            PerformLocomotion();
        }

        private void OnSprint(CallbackContext context)
        {
            _player.Movement.IsWalking = true;
            _player.Movement.IsCrouching = false;
            _player.Movement.IsRunning = true;
            
            PerformLocomotion();
        }

        private void PerformLocomotion()
        {
            var mousePosition = _inputManager.Input.Mouse.Position.ReadValue<Vector2>();
            var ray = _camera.ScreenPointToRay(mousePosition);

            if (!Physics.Raycast(ray, out var hit)) return;
            
            // Check if point is on navmesh
            if (!NavMesh.SamplePosition(hit.point, out var navMeshHit, 1f, NavMesh.AllAreas)) return;
            
            _player.Movement.SetMovementDestination(navMeshHit.position);
            
            OnMove?.Invoke(hit);
        }

        public void EnableInput()
        {
            _inputManager.Input.Mouse.Position.EnableInputAction();
            _inputManager.Input.PlayerController.Map.EnableInputActionMap();
        }

        public void DisableInput()
        {
            _inputManager.Input.Mouse.Position.DisableInputAction();
            _inputManager.Input.PlayerController.Map.DisableInputActionMap();
        }
    }
}