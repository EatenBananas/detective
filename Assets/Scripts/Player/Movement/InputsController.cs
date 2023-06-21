using System;
using CameraSystem;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Player.Movement
{
    public class InputsController : MonoBehaviour
    {
        private PlayerInputActions _playerInputActions;
        private PlayerMovement _playerMovement;
        private bool _isMouseOverUI;

        private void OnEnable() => _playerInputActions.Enable();
        private void OnDisable() => _playerInputActions.Disable();

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Player.Sneak.started += OnStartedSneak;
            _playerInputActions.Player.Sneak.canceled += OnCanceledSneak;
            _playerInputActions.Player.Walk.performed += OnWalk;
            _playerInputActions.Player.Sprint.performed += OnSprint;
        }

        private void Update()
        {
            IsMouseOverUI();
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

        private bool GetMouseToWorldPosition(out Vector3 position)
        {
            var mousePosition = _playerInputActions.Player.MousePosition.ReadValue<Vector2>();
            var ray = CameraGetter.MainCamera.ScreenPointToRay(mousePosition);
            Physics.Raycast(ray, out var hit);

            if (_isMouseOverUI)
            {
                position = default;
                return false;
            }

            if (NavMesh.SamplePosition(hit.point, out var navMeshHit, 1f,
                    NavMesh.AllAreas))
            {
                Debug.Log($"nav: {NavMesh.GetAreaFromName("Walkable")}");
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
