using System;
using UnityEngine;
using UnityEngine.AI;

namespace Player.Movement
{
    public enum PlayerMovingState
    {
        Running,
        Walking,
        Sneaking
    }
    
    public class PlayerMovement : MonoBehaviour
    {
        public PlayerMovingState PlayerMovingState
        {
            get => _playerMovingState;
            set
            {
                _playerMovingState = value;
                SetPlayerSpeed(_playerMovingState);
            }
        }

        [SerializeField] private PlayerMovingState _playerMovingState;
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
                _agent.destination = newTargetPosition;
        }

        public void SetPlayerSpeed(PlayerMovingState playerMovingState)
        {
            switch (playerMovingState)
            {
                case PlayerMovingState.Running:
                    _agent.speed = _runSpeed;
                    break;
                case PlayerMovingState.Walking:
                    _agent.speed = _walkSpeed;
                    break;
                case PlayerMovingState.Sneaking:
                    _agent.speed = _sneakSpeed;
                    break;
                default:
                    break;
            }
        }
    }
}
