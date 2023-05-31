using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Enemy
{
    public enum MovementType
    {
        Path,
        Random
    }
    
    public class EnemyMovement : MonoBehaviour
    {
        public UnityEvent OnWalkInTarget;
        public MovementType MovementType => _movementType;
        public Transform AgentWalkTarget { get; private set; }
        public List<Transform> WalkTargets => _walkTargets;

        [SerializeField] private MovementType _movementType;
        [SerializeField] private List<Transform> _walkTargets;

        private NavMeshAgent _agent;
        private int _agentWalkTargetIndex;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false;              // Fix agent slow rotation
            
            OnWalkInTarget.AddListener(UpdateTarget);
            
            UpdateTarget();
        }

        private void LateUpdate()
        {
            // Fix agent slow rotation
            if (_agent.velocity.sqrMagnitude > Mathf.Epsilon) 
                transform.rotation = Quaternion.LookRotation(_agent.velocity.normalized);
        }

        private void UpdateTarget()
        {
            if (_agentWalkTargetIndex >= WalkTargets.Count-1) _agentWalkTargetIndex = 0;
            else _agentWalkTargetIndex++;

            AgentWalkTarget = WalkTargets[_agentWalkTargetIndex];

            switch (MovementType)
            {
                case MovementType.Path:
                    MoveByPath();
                    break;
                case MovementType.Random:
                    MoveByRandom();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void MoveByPath()
        {
            _agent.destination = AgentWalkTarget.position;
        }

        private void MoveByRandom()
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform == AgentWalkTarget)
            {
                OnWalkInTarget.Invoke();
            }
        }

        private Transform GetClosestPoint()
        {
            var closestPoint = WalkTargets[0];
            var lesDistance = Vector3.Distance(transform.position, closestPoint.position);
            foreach (var point in WalkTargets)
            {
                var distance = Vector3.Distance(transform.position, point.position);
                if (!(distance < lesDistance)) continue;
                    
                closestPoint = point;
                lesDistance = distance;
            }
                
            return closestPoint;
        }
    }
}
