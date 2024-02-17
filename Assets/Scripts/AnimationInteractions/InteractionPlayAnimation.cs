using System.Threading.Tasks;
using GameInputSystem;
using InteractionSystem;
using PlayerSystem;
using ToolTipSystem;
using UnityEngine;
using Zenject;

namespace AnimationInteractions
{
    public class InteractionPlayAnimation : MonoBehaviour, IInteraction, IToolTip
    {
        
        
        [SerializeField] private bool _canPlayAnimation = true;
        [SerializeField] private string _animationName;
        [SerializeField] private Transform _startPoint;
        [SerializeField] private ToolTipData _toolTipData;
        
        [Inject] private Player _player;
        [Inject] private InputManager _inputManager;

        private async void HandleAnimation()
        {
            _canPlayAnimation = false;
            
            _inputManager.Input.PlayerController.Move.DisableInputAction();
            
            _player.Movement.SetMovementDestination(_startPoint.position);

            while (_player.Movement.Agent.hasPath) await Task.Delay(100);
            
            _player.Movement.Animator.Play(_animationName);
            
            _inputManager.Input.PlayerController.Move.EnableInputAction();

            _canPlayAnimation = true;
        }
        
        #region IToolTip
        
        public ToolTipData ToolTipData => _toolTipData;

        #endregion

        #region IIteraction

        public bool CanInteract() => _canPlayAnimation;

        public void OnEnter() { }

        public void OnStay() { }

        public void OnDown() => HandleAnimation();

        public void OnExit() { }

        #endregion
    }
}