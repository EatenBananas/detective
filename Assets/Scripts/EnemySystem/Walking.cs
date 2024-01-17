using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PlayerSystem;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Vector3;

namespace EnemySystem
{
    public enum WaypointActionType
    {
        InstantlyGoToNextWaypoint,
        WalkTo,
        Wait,
        NOT_IMPLEMENTED_LookAt,
        NOT_IMPLEMENTED_PlayAnimation,
        NOT_IMPLEMENTED_PlaySound,
        NOT_IMPLEMENTED_SpawnObject,
        NOT_IMPLEMENTED_DestroyObject,
        NOT_IMPLEMENTED_DestroySelf,
    }
    
    [Serializable]
    public class WaypointAction
    {
        public WaypointActionType ActionType;

        [ShowIf("ActionType", WaypointActionType.WalkTo)]
        public Transform Destination;

        [ShowIf("ActionType", WaypointActionType.Wait)]
        public float WaitTime;

        [ShowIf("ActionType", WaypointActionType.NOT_IMPLEMENTED_LookAt)]
        public Transform LookAtTarget;

        [ShowIf("ActionType", WaypointActionType.NOT_IMPLEMENTED_PlayAnimation)]
        public AnimationClip Animation;

        [ShowIf("ActionType", WaypointActionType.NOT_IMPLEMENTED_PlaySound)]
        public AudioClip Sound;

        [ShowIf("ActionType", WaypointActionType.NOT_IMPLEMENTED_SpawnObject)]
        public GameObject ObjectPrefab;

        [ShowIf("ActionType", WaypointActionType.NOT_IMPLEMENTED_DestroyObject)]
        public GameObject ObjectToDestroy;
    }

    [Serializable]
    public class Waypoint
    {
        [Space(15)]
        public Transform Point;
        public List<WaypointAction> WaypointActions;
    }

    public class Walking : MonoBehaviour
    {
        [SerializeField] private HumanoidMovement _movement;
        [SerializeField] private float _distanceToSwitchWaypoint;
        [SerializeField] private List<Waypoint> _waypoints;

        private int _currentWaypointIndex;
        private bool _isActionPending;

        #region Unity Lifecycle

        #region OnValidate

        private void OnValidate()
        {
            ValidateWaypointsNames();
        }
        
        private void ValidateWaypointsNames()
        {
            if (_waypoints.Count == 0) return;
            for (var i = 0; i < _waypoints.Count; i++)
            {
                if (_waypoints[i] == null) continue;
                if (_waypoints[i].Point == null) continue;
                if (_waypoints[i].Point.gameObject.name == $"Waypoint {i}") continue;
                _waypoints[i].Point.gameObject.name = $"Waypoint {i}";
            }
        }

        #endregion

        private void Start()
        {
            _currentWaypointIndex = GetClosestWaypointIndex();
            InitWalking();
        }

        
        #endregion

        private async void InitWalking()
        {
            while (_waypoints.Count > 0) 
                await WalkingLoop();
        }
        
        private async Task WalkingLoop()
        {
            if (_waypoints.Count == 0) return;

            if (_movement.Agent.path.corners[0] != _waypoints[_currentWaypointIndex].Point.position)
            {
                await WalkTo(_waypoints[_currentWaypointIndex].Point.position);
                
                if (_waypoints[_currentWaypointIndex].WaypointActions.Count > 0)
                    foreach (var action in _waypoints[_currentWaypointIndex].WaypointActions) 
                        await DoWaypointAction(_waypoints[_currentWaypointIndex], action);
            }
            
            _currentWaypointIndex = GetNextWaypointIndex(_currentWaypointIndex);
        }
        
        private async Task WalkTo(Vector3 destination)
        {
            _movement.SetMovementDestination(destination);

            while (true)
            {
                var isOnDestination = Distance(_movement.Agent.transform.position, destination) <=
                                      _distanceToSwitchWaypoint;

                await Task.Delay(100);

                if (isOnDestination) break;
            }
        }

        private async Task DoWaypointAction(Waypoint waypoint, WaypointAction action)
        {
            _isActionPending = true;
            
            switch (action.ActionType)
            {
                case WaypointActionType.InstantlyGoToNextWaypoint:
                    _isActionPending = false;
                    break;
                
                case WaypointActionType.WalkTo:
                    var destinationPosition = action.Destination.position;
                    await WalkTo(destinationPosition);
                    var agentPosition = _movement.Agent.transform.position;
                    var distanceToDestination = Distance(agentPosition, destinationPosition);
                    await Task.Run(() => { while (distanceToDestination > _distanceToSwitchWaypoint) { } });
                    _isActionPending = false;
                    break;
                
                case WaypointActionType.Wait:
                    await Task.Delay(TimeSpan.FromSeconds(action.WaitTime));
                    _isActionPending = false;
                    break;
                
                default:
                    _isActionPending = false;
                    break;
            }
            
            await Task.Run(() => { while (_isActionPending) { } });
        }
        
        private int GetNextWaypointIndex(int currentWaypointIndex) => (currentWaypointIndex + 1) % _waypoints.Count;
        
        private int GetClosestWaypointIndex()
        {
            var closestDistance = float.MaxValue;
            var closestIndex = 0;
            
            for (var i = 0; i < _waypoints.Count; i++)
            {
                var distance = Distance(_movement.transform.position, _waypoints[i].Point.position);
                
                if (!(distance < closestDistance)) continue;
                
                closestDistance = distance;
                closestIndex = i;
            }

            return closestIndex;
        }

#region Debug
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            DrawWaypoints();
        }
        
        private void DrawWaypoints()
        {
            if (_waypoints.Count == 0) return;
            
            
            for (var i = 0; i < _waypoints.Count; i++)
            {
                if (_waypoints[i] == null) continue;
                if (_waypoints[i].Point == null) continue;
                var waypointPosition = _waypoints[i].Point.position;
                
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(waypointPosition, 0.2f);
                var labelPosition = waypointPosition + one * 0.2f;
                Handles.Label(labelPosition, $"Waypoint {i}");

                Gizmos.color = Color.gray;
                Gizmos.DrawLine(waypointPosition, _waypoints[GetNextWaypointIndex(i)].Point.position);
            }
        }
#endif
#endregion
    }
}