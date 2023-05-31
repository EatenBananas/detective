using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Enemy
{
    public enum MovementType
    {
        Path,
        Random
    }
    
    public class EnemyMovement : MonoBehaviour
    {
        public List<Transform> WalkToPoints => _walkToPoints;
        public ScrollRect.MovementType MovementType
        {
            get => _movementType;
            set => _movementType = value;
        }

        public Transform LastAgentPoint
        {
            get
            {
                if (_lastAgentPoint == null)
                {
                    var closestPoint = WalkToPoints[0];
                    var lesDistance = Vector3.Distance(transform.position, closestPoint.position);
                    foreach (var point in WalkToPoints)
                    {
                        var distance = Vector3.Distance(transform.position, point.position);
                        if (distance < lesDistance)
                        {
                            closestPoint = point;
                            lesDistance = distance;
                        }
                    }

                    return closestPoint;
                }

                return _lastAgentPoint;
            }
            set => _lastAgentPoint = value;
        }

        [SerializeField] private List<Transform> _walkToPoints;
        [SerializeField] private ScrollRect.MovementType _movementType;
        
        private NavMeshAgent _agent;
        private Transform _lastAgentPoint;
        
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void MoveByPath()
        {
            // _agent.destination = _walkToPoints[_lastAgentPoint].position;
        }
    }
}
