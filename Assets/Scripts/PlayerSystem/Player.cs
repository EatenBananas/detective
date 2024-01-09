using GameInputSystem;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Zenject;

namespace PlayerSystem
{
    [SelectionBase]
    public class Player : MonoBehaviour
    {
        [field: SerializeField] public NavMeshAgent Agent { get; private set; }
        [field: SerializeField] public HumanoidMovement Movement { get; private set; }

        [Inject] private InputManager _inputManager;
        [Inject] private Camera _camera;
        
        private void OnEnable()
        {
            _inputManager.Input.PlayerController.Move.performed += OnMovePerformed;
            
            _inputManager.Input.PlayerController.Move.EnableInputAction();
            _inputManager.Input.Mouse.Position.EnableInputAction();
        }
        
        private void OnDisable()
        {
            _inputManager.Input.PlayerController.Move.performed -= OnMovePerformed;
            
            _inputManager.Input.PlayerController.Move.DisableInputAction();
            _inputManager.Input.Mouse.Position.DisableInputAction();
        }
        
        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            var mousePosition = _inputManager.Input.Mouse.Position.ReadValue<Vector2>();
            var ray = _camera.ScreenPointToRay(mousePosition);

            if (!Physics.Raycast(ray, out var hit)) return;
            
            Movement.SetMovementDestination(hit.point);
        }
        
        public void Teleport(Vector3 position)
        {
            transform.position = position;
        }
    }
}
