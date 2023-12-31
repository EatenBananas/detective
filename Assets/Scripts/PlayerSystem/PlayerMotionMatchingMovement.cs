using GameInputSystem;
using ModestTree;
using UnityEditor;
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
        public Vector2 Velocity
        {
            get => _velocity;
            private set => _velocity = value.normalized;
        }

        [SerializeField] private Animator _animator;
        [SerializeField] private NavMeshAgent _agent;
        
        [Inject] private InputManager _inputManager;
        [Inject] private Camera _camera;

        private Vector2 _smoothDeltaPosition;
        private Vector2 _velocity;
        private Vector3 _destination;
        private bool _isStopped = true;
        
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

        private void SynchronizeAnimatorWithAgent()
        {
            if (_agent.path.corners.Length < 2) return;
            
            var animatorTransform = _animator.transform;
            var directionFromAnimatorToFirstCornerOfAgentPath = _agent.path.corners[1] - animatorTransform.position;
            directionFromAnimatorToFirstCornerOfAgentPath.y = 0;
            
            var deltaX = Vector3.Dot(animatorTransform.right, directionFromAnimatorToFirstCornerOfAgentPath);
            var deltaY = Vector3.Dot(animatorTransform.forward, directionFromAnimatorToFirstCornerOfAgentPath);
            var deltaPosition = new Vector2(deltaX, deltaY);
            
            var smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
            _smoothDeltaPosition = Vector2.Lerp(_smoothDeltaPosition, deltaPosition, smooth);
            
            Velocity = deltaPosition / Time.deltaTime;
            if (_agent.remainingDistance <= _agent.stoppingDistance)
                Velocity = Vector2.Lerp(Velocity, Vector2.zero, _agent.remainingDistance / _agent.stoppingDistance);
            
            CheckIfIsMoving();
            SetLocomotionDirection(Velocity);
            
            var deltaMagnitude = directionFromAnimatorToFirstCornerOfAgentPath.magnitude;
            if (deltaMagnitude > _agent.radius / 2)
                _animator.transform.position = Vector3.Lerp(_animator.rootPosition, _agent.path.corners[0], smooth);
        }

        private void SetLocomotionDirection(Vector2 direction)
        {
            _animator.SetFloat(LocomotionHorizontal, direction.x);
            _animator.SetFloat(LocomotionVertical, direction.y);
        }
        
        private void CheckIfIsMoving()
        {
            var isMoving = _agent.velocity.magnitude > 0.1f && _agent.remainingDistance > _agent.radius;
            
            _animator.SetBool(IsMoving, isMoving);
        }

        public void LegToStopWalk(string legName) {  }
        
        #region Debug
        
        private void OnDrawGizmos()
        {
            // Draw point where user clicked
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_destination, 0.2f);
            Handles.Label(_destination, "Destination");
            
            // Draw agent destination
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_agent.destination, 0.2f);
            Handles.Label(_agent.destination, "Agent Destination");

            foreach (var corner in _agent.path.corners)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(corner, 0.1f);
                Handles.Label(corner, _agent.path.corners.IndexOf(corner).ToString());
            }
        }
        
        #endregion
    }
}
