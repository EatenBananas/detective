using System;
using UnityEngine;
using UnityEngine.AI;
using Debug = System.Diagnostics.Debug;

namespace Prototypes
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PrototypeMovement : MonoBehaviour {
        
        private NavMeshAgent _agent;
        private const float MAX_DISTANCE = 100;

        private void Start() {
            _agent = GetComponent<NavMeshAgent>();
        }
        
        private  void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            Debug.Assert(Camera.main != null, "Camera.main != null");
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, MAX_DISTANCE)) {
                _agent.destination = hit.point;
            }
        }

        public void Lock()
        {
            _agent.enabled = false;
        }

        public void Unlock()
        {
            _agent.enabled = true;
        }
    }
}