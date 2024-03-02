using System.Threading.Tasks;
using GameInputSystem;
using GameManagers;
using UnityEngine;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

namespace InteractionSystem
{
    public class InteractionManager : MonoBehaviour
    {
        [Inject] private RayCaster _rayCaster;
        [Inject] private InputManager _inputManager;

        private IInteraction _currentInteraction;

        private void OnEnable() => 
            _rayCaster.OnLookingAtObjectBehindTheMouseChange += OnLookingAtObjectBehindTheMouseChange;

        private void OnDisable() => 
            _rayCaster.OnLookingAtObjectBehindTheMouseChange -= OnLookingAtObjectBehindTheMouseChange;

        private void OnDestroy() => EndInteraction();


        private void OnLookingAtObjectBehindTheMouseChange(GameObject obj)
        {
            if (obj != null && obj.TryGetComponent(out IInteraction interaction))
            {
                _inputManager.Input.PlayerController.Map.DisableInputActionMap();
                StartInteraction(interaction);
            }
            else
            {
                EndInteraction();
                _inputManager.Input.PlayerController.Map.EnableInputActionMap();
            }
        }
        
        private async void StartInteraction(IInteraction interaction)
        {
            _currentInteraction = interaction;
            
            while (!interaction.CanInteract()) await Task.Delay(100);

            interaction.OnEnter();
            
            _inputManager.Input.Interaction.Interact.performed += Interact;

            while (_currentInteraction == interaction)
            {
                if (interaction.CanInteract())
                {
                    interaction.OnStay();
                    
                    _inputManager.Input.Interaction.Interact.EnableInputAction();
                }
                else _inputManager.Input.Interaction.Interact.DisableInputAction();
                
                await Task.Delay(100);
            }

            _inputManager.Input.Interaction.Interact.performed -= Interact;
            
            interaction.OnExit();
        }

        private void EndInteraction()
        {
            _currentInteraction = null;
        }

        private void Interact(CallbackContext callbackContext)
        {
            if (_currentInteraction == null) return;
            if (!_currentInteraction.CanInteract()) return;
            
            _currentInteraction?.OnDown();
        }
    }
}