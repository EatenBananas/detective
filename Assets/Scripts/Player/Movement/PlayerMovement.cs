using UnityEngine;
using UnityEngine.AI;

namespace Player.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
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
    }
}
