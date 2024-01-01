using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace PlayerSystem
{
    public class PlayerPlayAnimation : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Animator _animator;
        
        [Button]
        private void Sit(Transform startSittingPoint)
        {
            // Walk to startSittingPoint
            _agent.SetDestination(startSittingPoint.position);
            
            // StartSitting
            _animator.SetBool("isSitting", true);
        }
        
        [Button]
        private void Stand()
        {
            _animator.SetBool("isSitting", false);
        }
    }
}