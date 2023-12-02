using System;
using GameManagers;
using InputSystem;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Camera = UnityEngine.Camera;
using Mouse = UnityEngine.InputSystem.Mouse;

namespace PlayerSystem
{
    public class OldPlayerMovement : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Vector3 _destination;
        
        [Inject] private InputManager _inputManager;
        [Inject] private Camera _camera;

        private void OnEnable()
        {
            _inputManager.Input.Player.Move.performed += OnMovePerformed;
        }
        
        private void OnDisable()
        {
            _inputManager.Input.Player.Move.performed -= OnMovePerformed;
        }
        
        private void OnMovePerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            var input = Mouse.current.position.value;
            var ray = _camera.ScreenPointToRay(input);
            
            if (Physics.Raycast(ray, out var hit))
            {
                _destination = hit.point;
                _agent.SetDestination(_destination);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(_destination, 1);
        }
    }
}
