using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InteractionSystem;
using PlayerSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
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
        PlayAnimation,
        SetAnimationTrigger,
        ResetAnimationTrigger
    }
    
    [Serializable]
    public class WaypointAction
    {
        public WaypointActionType ActionType;

        [ShowIf("ActionType", WaypointActionType.WalkTo)]
        public Transform Destination;

        [ShowIf("ActionType", WaypointActionType.Wait)]
        public float WaitTime;

        [ShowIf(nameof(canShowAnimator))]
        public Animator Animator;
        
        [ShowIf("ActionType", WaypointActionType.PlayAnimation)]
        public string AnimationName;

        [ShowIf(nameof(canShowTriggerName))]
        public string TriggerName;


        private bool canShowAnimator => ActionType is WaypointActionType.PlayAnimation
            or WaypointActionType.SetAnimationTrigger or WaypointActionType.ResetAnimationTrigger;
        
        private bool canShowTriggerName => ActionType is WaypointActionType.SetAnimationTrigger
            or WaypointActionType.ResetAnimationTrigger;
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
        [SerializeField] private bool _looping;
        [SerializeField] private List<Waypoint> _waypoints;

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
            InitWalking();
        }

        #endregion

        private async void InitWalking()
        {
            await WalkingLoop();
            
            if (_looping) InitWalking();
        }
        
        private async Task WalkingLoop()
        {
            foreach (var waypoint in _waypoints)
            {
                await WalkTo(waypoint.Point.position);

                foreach (var action in waypoint.WaypointActions) 
                    await DoWaypointAction(waypoint, action);
            }
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
                    break;
                
                case WaypointActionType.WalkTo:
                    await WalkTo(action.Destination.position);
                    break;
                
                case WaypointActionType.Wait:
                    await Task.Delay(TimeSpan.FromSeconds(action.WaitTime));
                    break;

                case WaypointActionType.PlayAnimation:
                    action.Animator.Play(action.AnimationName);
                    break;
                
                case WaypointActionType.SetAnimationTrigger:
                    action.Animator.SetTrigger(action.TriggerName);
                    break;
                
                case WaypointActionType.ResetAnimationTrigger:
                    action.Animator.ResetTrigger(action.TriggerName);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _isActionPending = false;
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