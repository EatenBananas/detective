using System;
using GameInputSystem;
using ModestTree;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Camera = UnityEngine.Camera;

namespace PlayerSystem
{
    public enum LocomotionState
    {
        None,
        StartingStart,
        StartingEnd,
        WalkingStart,
        WalkingEnd,
        StoppingStart,
        StoppingEnd,
    }
    
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

        private bool _isMoving => _agent.velocity.magnitude > 0.1f;

        [SerializeField] private Animator _animator;
        [SerializeField] private NavMeshAgent _agent;
        
        [Inject] private InputManager _inputManager;
        [Inject] private Camera _camera;

        private Vector2 _smoothDeltaPosition;
        private Vector2 _velocity;
        private Vector3 _destination;
        private bool _isStopped = true;
        private LocomotionState _locomotionState = LocomotionState.None;
        private bool _doOnceLocomotionStart;
        private bool _doOnceLocomotionEnd;
        
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
            HandleLocomotion();
        }

        #endregion
        
        private void OnMovePerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            var mousePosition = _inputManager.Input.Mouse.Position.ReadValue<Vector2>();
            var ray = _camera.ScreenPointToRay(mousePosition);

            if (!Physics.Raycast(ray, out var hit)) return;
            
            _destination = hit.point;
            _agent.SetDestination(_destination);
            
            _doOnceLocomotionStart = false;
            _doOnceLocomotionEnd = false;
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


        private void HandleLocomotion()
        {
            if (_isMoving)
            {
                _animator.SetBool(IsMoving, _isMoving);
            }
            else
            {
                _animator.SetBool(IsMoving, _isMoving);
                return;
            }
            
            switch (_locomotionState)
            {
                case LocomotionState.None:
                    break;
                case LocomotionState.StartingStart:
                    LocomotionStart();
                    break;
                case LocomotionState.StartingEnd:
                    break;
                case LocomotionState.WalkingStart:
                    LocomotionIn();
                    break;
                case LocomotionState.WalkingEnd:
                    break;
                case LocomotionState.StoppingStart:
                    LocomotionEnd();
                    break;
                case LocomotionState.StoppingEnd:
                    break;
                default:
                    return;
            }
        }
        private void LocomotionStart()
        {
            if (_doOnceLocomotionStart) return;
            if (_agent.path.corners.Length < 2) return;
            
            var locomotionDirection = CalculateLocomotionDirection(_agent.nextPosition);

            SetLocomotionDirection(locomotionDirection);
            _doOnceLocomotionStart = true;
        }
        private void LocomotionIn()
        {
            if (_agent.path.corners.Length < 2) return;
            
            var locomotionDirection = CalculateLocomotionDirection(_agent.nextPosition);

            SetLocomotionDirection(locomotionDirection);
        }
        private void LocomotionEnd()
        {
            if (_doOnceLocomotionEnd) return;
            if (_agent.path.corners.Length < 2) return;
            
            var locomotionDirection = CalculateLocomotionDirection(_agent.nextPosition);
            
            SetLocomotionDirection(locomotionDirection);
            _doOnceLocomotionEnd = true;
        }
        

        private Vector2 CalculateLocomotionDirection(Vector3 targetPosition)
        {
            var animatorTransform = _animator.transform;
            var directionFromAnimatorToTargetPosition = targetPosition - animatorTransform.position;
            directionFromAnimatorToTargetPosition.y = 0;
            
            var deltaX = Vector3.Dot(animatorTransform.right, directionFromAnimatorToTargetPosition);
            var deltaY = Vector3.Dot(animatorTransform.forward, directionFromAnimatorToTargetPosition);
            var deltaPosition = new Vector2(deltaX, deltaY);
            
            return deltaPosition.normalized;
        }
        
        private void SetLocomotionDirection(Vector2 direction)
        {
            _animator.SetFloat(LocomotionHorizontal, direction.x);
            _animator.SetFloat(LocomotionVertical, direction.y);
        }

        private void OnLocomotionStateChange(string state)
        {
            _locomotionState = (LocomotionState)Enum.Parse(typeof(LocomotionState), state);
        }
        
        private void LegToStopWalk()
        {
        }

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
            
            // Draw agent next position
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_agent.nextPosition, 0.2f);
            Handles.Label(_agent.nextPosition, "Agent Next Position");
            
            // Draw agent path
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
