using System;
using UnityEngine;
using UnityEngine.AI;

namespace PlayerSystem
{
    public class PlayerMotionMatchingMovement : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private NavMeshAgent _navMeshAgent;

        private void Awake()
        {
            _animator.applyRootMotion = true;
            _navMeshAgent.updatePosition = false;
            _navMeshAgent.updateRotation = false;
        }

        private void OnAnimatorMove()
        {
            // This is a hack to make the NavMeshAgent follow the root motion of the animator.
            var rootPosition = _animator.rootPosition;
            rootPosition.y = _navMeshAgent.nextPosition.y;
            _navMeshAgent.nextPosition = rootPosition;
            var navMeshAgentTransform = _navMeshAgent.transform;
            navMeshAgentTransform.position = rootPosition;
            navMeshAgentTransform.rotation = _animator.rootRotation;
        }
        
        private void SynchronizeAnimatorWithNavMeshAgent()
        {
            // This is a hack to make the animator follow the NavMeshAgent.
            _animator.rootPosition = _navMeshAgent.nextPosition;
            _animator.rootRotation = _navMeshAgent.transform.rotation;
        }
    }
}
