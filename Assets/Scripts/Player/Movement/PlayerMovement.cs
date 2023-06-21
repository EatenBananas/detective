using System;
using UnityEngine;
using UnityEngine.AI;

namespace Player.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        public PlayerMovingState PlayerMovingState { get; private set; }
        public Vector3 LastPlayerTargetPosition => _agent.destination;

        public bool MovementBlocked
        {
            get => _movementBlocked;
            set
            {
                _movementBlocked = value;
                if (!_movementBlocked) SetPlayerTargetPosition(_agent.transform.position);
            }
        }

        [SerializeField] private string _walkableArea = "Walkable";
        [SerializeField] private float _walkSpeed;
        [SerializeField] private float _runSpeed;
        [SerializeField] private float _sneakSpeed;
        
        private static NavMeshAgent _agent;
        private bool _movementBlocked;

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
            if (MovementBlocked) return;

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