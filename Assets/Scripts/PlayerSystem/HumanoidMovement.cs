using System;
using ModestTree;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

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
    
    public class HumanoidMovement : MonoBehaviour   
    {
        public bool CamPlayerMove
        {
            get => _camPlayerMove;
            set
            {
                if (_camPlayerMove == value) return;
                _agent.isStopped = value;
                _camPlayerMove = value;
            }
        }
        public bool IsAgentMoving => _agent.velocity.magnitude > 0.1f;
        public NavMeshAgent Agent => _agent;

        [SerializeField] private Animator _animator;
        [SerializeField] private NavMeshAgent _agent;

        private Vector2 _smoothDeltaPosition;
        private Vector2 _velocity;
        private bool _camPlayerMove;
        private LocomotionState _locomotionState = LocomotionState.None;
        private bool _doOnceLocomotionStart;
        private bool _doOnceLocomotionEnd;
        
        private static readonly int IsMoving = Animator.StringToHash("isMoving");
        private static readonly int LocomotionHorizontal = Animator.StringToHash("locomotionHorizontal");
        private static readonly int LocomotionVertical = Animator.StringToHash("locomotionVertical");
        private static readonly int IsLeftFoot = Animator.StringToHash("isLeftFoot");

        #region Unity Lifecycle

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
        
        public void SetMovementDestination(Vector3 destination)
        {
            _agent.SetDestination(destination);
            
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
            _animator.SetBool(IsMoving, IsAgentMoving);
            if (!IsAgentMoving) return;
            
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
        
        private void LegToStopWalk(string upperLeg)
        {
            switch (upperLeg)
            {
                case "Left":
                    _animator.SetBool(IsLeftFoot, true);
                    break;
                case "Right":
                    _animator.SetBool(IsLeftFoot, false);
                    break;
                default:
                    return;
            }
        }
        
        public void StopPlayer()
        {
            _agent.velocity = Vector3.zero;
            _agent.SetDestination(_agent.transform.position);
        }

        #region Debug
        
        private void OnDrawGizmos()
        {
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
