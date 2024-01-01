using System.Threading.Tasks;
using GameInputSystem;
using Sirenix.OdinInspector;
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
        
        private static readonly int IsMoving = Animator.StringToHash("isMoving");
        private static readonly int LocomotionHorizontal = Animator.StringToHash("locomotionHorizontal");
        private static readonly int LocomotionVertical = Animator.StringToHash("locomotionVertical");

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

        private void SetLocomotionDirection(Vector2 direction)
        {
            _animator.SetFloat(LocomotionHorizontal, direction.x);
            _animator.SetFloat(LocomotionVertical, direction.y);
        }
        
        private void SynchronizeAnimatorWithAgent()
        {
            var animatorTransform = _animator.transform;
            var worldDeltaPosition = _agent.path.corners[0] - animatorTransform.position;
            worldDeltaPosition.y = 0;
            
            var deltaX = Vector3.Dot(animatorTransform.right, worldDeltaPosition);
            var deltaY = Vector3.Dot(animatorTransform.forward, worldDeltaPosition);
            var deltaPosition = new Vector2(deltaX, deltaY);
            
            var smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
            _smoothDeltaPosition = Vector2.Lerp(_smoothDeltaPosition, deltaPosition, smooth);
            
            _velocity = _smoothDeltaPosition / Time.deltaTime;
            if (_agent.remainingDistance <= _agent.stoppingDistance)
                _velocity = Vector2.Lerp(_velocity, Vector2.zero, _agent.remainingDistance / _agent.stoppingDistance);
            
            var isWalking = _velocity.magnitude > 0.5f && _agent.remainingDistance > _agent.stoppingDistance;
            
            _animator.SetBool(IsMoving, isWalking);
            _animator.SetFloat(LocomotionHorizontal, _velocity.x);
            _animator.SetFloat(LocomotionVertical, _velocity.y);
            
            var deltaMagnitude = worldDeltaPosition.magnitude;
            if (deltaMagnitude > _agent.radius / 2)
                _animator.transform.position = Vector3.Lerp(_animator.rootPosition, _agent.path.corners[0], smooth);
        }

        public void LegToStopWalk(string legName)
        {
            
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

            foreach (var corner in _agent.path.corners)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(corner, 0.5f);
            }
        }
        
        #endregion
    }
}
