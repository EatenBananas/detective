using System.Threading.Tasks;
using GameInputSystem;
using InteractionSystem;
using PlayerSystem;
using ToolTipSystem;
using UnityEngine;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

namespace AnimationInteractions
{
    public class InteractionSetAnimationTrigger : MonoBehaviour, IInteraction, IToolTip
    {
        [SerializeField] private bool _canPlayAnimation = true;
        [SerializeField] private string _triggerName;
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _lookAtPoint;
        [SerializeField] private ToolTipData _toolTipData;
        
        [Inject] private Player _player;
        [Inject] private InputManager _inputManager;

        private async void HandleInteraction()
        {
            _inputManager.Input.PlayerController.Move.DisableInputAction();
            
            _canPlayAnimation = false;
            
            _player.Movement.SetMovementDestination(_startPoint.position);

            do await Task.Delay(100);
            while (_player.Movement.IsAgentMoving);

            var directionToLookAtPoint = _lookAtPoint.position - _player.transform.position;
            _player.transform.forward = directionToLookAtPoint;
            
            _player.Movement.Animator.SetTrigger(_triggerName);
            
            InitCancel();
            
            _inputManager.Input.PlayerController.Move.EnableInputAction();
        }

        private void Cancel(CallbackContext callbackContext)
        {
            _player.Movement.Animator.ResetTrigger(_triggerName);
            
            DeInitCancel();
        }

        private void InitCancel()
        {
            _inputManager.Input.PlayerController.Move.performed += Cancel;
        }

        private void DeInitCancel()
        {
            _inputManager.Input.PlayerController.Move.performed -= Cancel;
        }

        #region IToolTip
        
        public ToolTipData ToolTipData => _toolTipData;

        #endregion

        #region IIteraction

        public bool CanInteract() => _canPlayAnimation;

        public void OnEnter() { }

        public void OnStay() { }

        public void OnDown() => HandleInteraction();

        public void OnExit() { }

        #endregion
    }
}