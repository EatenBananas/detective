using System;
using UnityEngine;
using UnityEngine.AI;

namespace Player.Movement
{
    public enum PlayerMovingState
    {
        Standing,
        Sneaking,
        Walking,
        Sprinting
    }
    
    public class PlayerMovement : MonoBehaviour
    {
        public static PlayerMovingState PlayerMovingState { get; private set; }
        public static Vector3 LastPlayerTargetPosition => _agent.destination;
        
        [SerializeField] private float _walkSpeed;
        [SerializeField] private float _runSpeed;
        [SerializeField] private float _sneakSpeed;
        
        private static NavMeshAgent _agent;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>(); 
            _agent.updateRotation = false;          // Fix agent slow rotation
        }

        private void LateUpdate()
        {
            // Fix slow agent rotation
            if (_agent.velocity.sqrMagnitude > Mathf.Epsilon)
                transform.rotation = Quaternion.LookRotation(_agent.velocity.normalized);
        }

        public void SetPlayerTargetPosition(Vector3 newTargetPosition)
        {
            // Move agent to position
            if (_agent.isOnNavMesh) 
                _agent.destination = newTargetPosition;
        }

        public void SetPlayerMovingState(PlayerMovingState playerMovingState)
        {
            switch (playerMovingState)
            {
                case PlayerMovingState.Standing:
                    break;
                case PlayerMovingState.Sneaking:
                    _agent.speed = _sneakSpeed;
                    break;
                case PlayerMovingState.Walking:
                    _agent.speed = _walkSpeed;
                    break;
                case PlayerMovingState.Sprinting:
                    _agent.speed = _runSpeed;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(playerMovingState), playerMovingState, null);
            }

            PlayerMovingState = playerMovingState;
        }
    }
}