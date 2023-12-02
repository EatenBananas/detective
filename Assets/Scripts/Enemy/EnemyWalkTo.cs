using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyWalkTo : MonoBehaviour
    {
        public NavMeshAgent _NavMeshAgent;
        
        [Button]
        private void MoveToTarget(Vector3 agentWalkTarget)
        {
            _NavMeshAgent.SetDestination(agentWalkTarget);
        }
    }
}