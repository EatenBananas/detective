using GameInputSystem;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Camera = UnityEngine.Camera;

namespace PlayerSystem
{
    public class PlayerMotionMatchingMovement : MonoBehaviour
    {
        public bool IsStopped
        {
            get => _isStopped;
            set
            {
                if (_isStopped == value) return;
                _agent.isStopped = value;
                _isStopped = value;
            }
        }

        private bool _isStopped = true;
        
        [SerializeField] private Animator _animator;
        [SerializeField] private NavMeshAgent _agent;
        
        [Inject] private InputManager _inputManager;
        [Inject] private Camera _camera;
        
        private Vector2 _smoothDeltaPosition = Vector2.zero;
        private Vector2 _velocity = Vector2.zero;
        private Vector3 _destination;
        
        private static readonly int IsWalking = Animator.StringToHash("isWalking");
        private static readonly int Horizontal = Animator.StringToHash("locomotionHorizontal");
        private static readonly int Vertical = Animator.StringToHash("locomotionVertical");

        #region Unity Lifecycle

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

        private void Awake()
        {
            _animator.applyRootMotion = true;
            _agent.updatePosition = false;
            _agent.updateRotation = false;
        }
        
        private void Update()
        {
            SynchronizeAnimatorWithAgent();
        }

        #endregion
        
        private void OnMovePerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            var mousePosition = _inputManager.Input.Mouse.Position.ReadValue<Vector2>();
            var ray = _camera.ScreenPointToRay(mousePosition);

            if (!Physics.Raycast(ray, out var hit)) return;
            
            _destination = hit.point;
            _agent.SetDestination(_destination);
        }

        private void OnAnimatorMove()
        {
            var animatorRootPosition = _animator.rootPosition;
            var animatorRootRotation = _animator.rootRotation;
            var agentTransform = _agent.transform;
            
            animatorRootPosition.y = _agent.nextPosition.y;
            
            agentTransform.position = animatorRootPosition;
            agentTransform.rotation = animatorRootRotation;
            
            _agent.nextPosition = animatorRootPosition;
        }
        
        private void SynchronizeAnimatorWithAgent()
        {
            var animatorTransform = _animator.transform;
            var worldDeltaPosition = _agent.nextPosition - animatorTransform.position;
            worldDeltaPosition.y = 0;
            
            var deltaX = Vector3.Dot(animatorTransform.right, worldDeltaPosition);
            var deltaY = Vector3.Dot(animatorTransform.forward, worldDeltaPosition);
            var deltaPosition = new Vector2(deltaX, deltaY);
            
            var smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
            _smoothDeltaPosition = Vector2.Lerp(_smoothDeltaPosition, deltaPosition, smooth);
            
            _velocity = _smoothDeltaPosition / Time.deltaTime;
            if (_agent.remainingDistance <= _agent.stoppingDistance)
                _velocity = Vector2.Lerp(_velocity, Vector2.zero, _agent.remainingDistance / _agent.stoppingDistance);
            
            // var isWalking = _velocity.magnitude > 0.5f && _agent.remainingDistance > _agent.stoppingDistance;
            var isWalking = _agent.velocity != Vector3.zero;
            
            if (_velocity.x > 1) _velocity.x = 1;
            if (_velocity.x < -1) _velocity.x = -1;
            if (_velocity.y > 1) _velocity.y = 1;
            if (_velocity.y < -1) _velocity.y = -1;
            
            _animator.SetBool(IsWalking, isWalking);
            _animator.SetFloat(Horizontal, _velocity.x);
            _animator.SetFloat(Vertical, _velocity.y);
            
            var deltaMagnitude = worldDeltaPosition.magnitude;
            if (deltaMagnitude > _agent.radius / 2)
                _animator.transform.position = Vector3.Lerp(_animator.rootPosition, _agent.nextPosition, smooth);
        }
        
        #region Debug
        
        private void OnDrawGizmos()
        {
            // Draw point where user clicked
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_destination, 0.1f);
            
            // Draw agent destination
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_agent.destination, 0.1f);
        }
        
        #endregion
    }
}
