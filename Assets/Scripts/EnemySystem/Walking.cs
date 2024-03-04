using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PlayerSystem;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Zenject;
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
        ResetAnimationTrigger,
        SetAnimationBool,
        SetMovement,
        RotateToTarget,
        Teleport
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
        
        [ShowIf(nameof(canShowAnimationName))]
        public string AnimationName;

        [ShowIf("ActionType", WaypointActionType.SetAnimationBool)]
        public bool AnimationBool;
        
        [ShowIf("ActionType", WaypointActionType.SetMovement)]
        public HumanoidMovement Movement;
        [ShowIf("ActionType", WaypointActionType.SetMovement)]
        public bool IsWalking;
        [ShowIf("ActionType", WaypointActionType.SetMovement)]
        public bool IsCrouching;
        [ShowIf("ActionType", WaypointActionType.SetMovement)]
        public bool IsRunning;
        
        [ShowIf("ActionType", WaypointActionType.RotateToTarget)]
        public NavMeshAgent Agent;
        [ShowIf("ActionType", WaypointActionType.RotateToTarget)]
        public Transform Target;
        
        [FormerlySerializedAs("TeleportPosition")] [ShowIf("ActionType", WaypointActionType.Teleport)]
        public Transform TeleportPoint;

        private bool canShowAnimationName => ActionType is WaypointActionType.PlayAnimation
            or WaypointActionType.SetAnimationTrigger
            or WaypointActionType.ResetAnimationTrigger or WaypointActionType.SetAnimationBool;

        private bool canShowAnimator => ActionType is WaypointActionType.PlayAnimation
            or WaypointActionType.SetAnimationTrigger or WaypointActionType.ResetAnimationTrigger
            or WaypointActionType.SetAnimationBool;
        
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
        private bool _canceled;
        [Indent] private Player _player;

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

        private void OnEnable()
        {
            _canceled = false;
            
            _player.Movement.Animator.Play("animName");
        }

        private void Start()
        {
            InitWalking();
        }
        
        private void OnDisable()
        {
            _canceled = true;
        }

        #endregion

        private async void InitWalking()
        {
            if (_canceled) return;
            
            await WalkingLoop();
            
            if (_looping) InitWalking();
        }
        
        private async Task WalkingLoop()
        {
            if (_canceled) return;
            
            foreach (var waypoint in _waypoints)
            {
                await WalkTo(waypoint.Point.position);

                foreach (var action in waypoint.WaypointActions) 
                    await DoWaypointAction(waypoint, action);
            }
        }
        
        private async Task WalkTo(Vector3 destination)
        {
            if (_canceled) return;
            
            _movement.SetMovementDestination(destination);

            while (true)
            {
                if (_canceled) break;
                
                var isOnDestination = Distance(_movement.Agent.transform.position, destination) <=
                                      _distanceToSwitchWaypoint;

                await Task.Delay(100);
                
                if (isOnDestination) break;
            }
        }

        private async Task DoWaypointAction(Waypoint waypoint, WaypointAction action)
        {
            if (_canceled)
            {
                _isActionPending = false;
                return;
            }
            
            _isActionPending = true;
            
            if (!_canceled)
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
                    action.Animator.SetTrigger(action.AnimationName);
                    break;
                
                case WaypointActionType.ResetAnimationTrigger:
                    action.Animator.ResetTrigger(action.AnimationName);
                    break;
                
                case WaypointActionType.SetAnimationBool:
                    action.Animator.SetBool(action.AnimationName, action.AnimationBool);
                    break;
                
                case WaypointActionType.SetMovement:
                    action.Movement.IsWalking = action.IsWalking;
                    action.Movement.IsCrouching = action.IsCrouching;
                    action.Movement.IsRunning = action.IsRunning;
                    break;
                
                case WaypointActionType.RotateToTarget:
                    action.Agent.enabled = false;
                    action.Agent.ResetPath();
                    action.Agent.transform.LookAt(action.Target.position);
                    action.Agent.enabled = true;
                    break;
                
                case WaypointActionType.Teleport:
                    transform.position = action.TeleportPoint.position;
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