using System;
using InteractionSystem;
using PlayerSystem;
using ToolTipSystem;
using UnityEngine;
using Zenject;

namespace AnimationInteractions.Desk
{
    public class DeskSitting : MonoBehaviour, IInteraction, IToolTip
    {
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _lookAtPoint;
        [SerializeField] private TriggerDetector _triggerDetector;

        [Inject] private Player _player;

        private bool _canTrySit;
        private bool _isSitting => _player.Movement.Animator.GetBool(_animIsSitting);
        
        private static readonly int _animIsSitting = Animator.StringToHash("isSitting");

        private void OnEnable()
        {
            _triggerDetector.OnStay += TrySit;
        }
        
        private void OnDisable()
        {
            _triggerDetector.OnStay -= TrySit;
        }

        private void GoToDesk()
        {
            _player.Movement.SetMovementDestination(_startPoint.position);
            _canTrySit = true;
        }
        
        private void TrySit(Collider other)
        {
            if (!_canTrySit) return;
            
            if (other.TryGetComponent(out Player player))
            {
                // Play sit animation
                var playerTransform = player.transform;
                playerTransform.forward = _lookAtPoint.position - playerTransform.position;
                player.Movement.Animator.SetBool(_animIsSitting, true);
            }
            
            _canTrySit = false;
        }
        
        #region IToolTip

        public ToolTipData ToolTipData
        {
            get
            {
                if (_isSitting)
                {
                    return new ToolTipData
                    {
                        Title = "Stand up",
                        Description = "Click on desk to stand up"
                    };
                }

                return new ToolTipData
                {
                    Title = "Sit",
                    Description = "Click on desk to sit"
                };
            }
        }

        #endregion
    
        #region IInteraction

        public bool CanInteract()
        {
            return true;    
        }

        public void OnEnter()
        {
        
        }

        public void OnStay()
        {
        
        }

        public void OnDown()
        {
            if (_isSitting)
            {
                _player.Movement.Animator.SetBool(_animIsSitting, false);
            }
            else GoToDesk();
        }

        public void OnExit()
        {
        
        }
    
        #endregion
    }
}
