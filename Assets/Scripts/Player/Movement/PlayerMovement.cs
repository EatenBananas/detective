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
        Running
    }
    
    public class PlayerMovement : MonoBehaviour
    {
        public PlayerMovingState PlayerMovingState { get; private set; }
        public Vector3 PlayerTargetPosition { get; private set; }
        
        [SerializeField] private float _walkSpeed;
        [SerializeField] private float _runSpeed;
        [SerializeField] private float _sneakSpeed;
        
        private NavMeshAgent _agent;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        public void SetPlayerTargetPosition(Vector3 newTargetPosition)
        {
            // Move agent to position
            if (_agent.isOnNavMesh)
            {
                _agent.destination = newTargetPosition;
                PlayerTargetPosition = newTargetPosition;
            }
        }

        public void SetPlayerSpeed(PlayerMovingState playerMovingState)
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
                case PlayerMovingState.Running:
                    _agent.speed = _runSpeed;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(playerMovingState), playerMovingState, null);
            }

            PlayerMovingState = playerMovingState;
        }
    }
}