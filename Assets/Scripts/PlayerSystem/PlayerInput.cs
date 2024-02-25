﻿using System;
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
            _inputManager.Input.PlayerController.Walk.performed += OnWalk;
            _inputManager.Input.PlayerController.Sprint.performed += OnSprint;
            
            EnableInput();
        }
        
        private void OnDisable()
        {
            _inputManager.Input.PlayerController.Walk.performed -= OnWalk;
            _inputManager.Input.PlayerController.Sprint.performed -= OnSprint;
            
            DisableInput();
        }
        
        private void OnSprint(CallbackContext context)
        {
            _player.Movement.IsRunning = true;
            
            PerformLocomotion();
        }
        
        private void OnWalk(CallbackContext context)
        {
            _player.Movement.IsRunning = false;
            
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