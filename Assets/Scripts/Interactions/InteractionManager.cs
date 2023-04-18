using System;
using TMPro;
using UnityEngine;

namespace Interactions
{
    public class InteractionManager : MonoBehaviour
    {
        // todo: replace with zenject
        public static InteractionManager Instance { get; private set; }
        
        private Interactable _interactable;
        private Interaction _interaction;
        private int _currentElementIndex;

        private KeyCode _keyCode;
        private bool _listenForKey = false;
        
        private void Start()
        {
            Instance = this;
        }
        
        public void Enter(Interactable interactable)
        {
            _interactable = interactable;
            UIManager.Instance.ShowInteractableText(_interactable.Text);
        }

        public void Exit()
        {
            UIManager.Instance.HideInteractableText();
            _interactable = null;
        }

        public void Update()
        {
            // todo: input handling logic should be moved somewhere else
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_interactable != null && _interaction == null)
                {
                    Debug.Log($"Starting {_interactable.name}");
                    StartInteraction(_interactable.Interaction);
                }
            }
            else if (_listenForKey && Input.GetKeyDown(_keyCode))
            {
                UIManager.Instance.HideDialogue();
                CompleteElement();
            }
        }

        public void StartInteraction(Interaction interaction)
        {
            _interaction = interaction;
            Debug.Log($"Starting {_interaction.name}");
            UIManager.Instance.HideInteractableText();
            _currentElementIndex = 0;
            StartElement();
        }

        private void StartElement()
        {
            var element = _interaction.Elements[_currentElementIndex];
            Debug.Log($"Starting {element.Type}");
            InteractionElement.ToElement(element).Execute();
        }

        public void CompleteElement()
        {
            Debug.Log("Completed");
            _currentElementIndex++;

            if (_currentElementIndex < _interaction.Elements.Count)
            {
                StartElement();
            }
            else
            {
                Debug.Log("Interaction completed");
                _interaction = null;
            }
        }

        // todo: move
        public void ListenForKey(KeyCode keyCode)
        {
            _listenForKey = true;
            _keyCode = keyCode;
        }
    }
}