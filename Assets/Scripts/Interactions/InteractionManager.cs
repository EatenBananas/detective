using System;
using System.Collections;
using System.Collections.Generic;
using Interactions.Elements;
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

        private List<Option> _activeOptions;
        private bool _listenForOptions = false;
        
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
                // temp here
                UIManager.Instance.HideDialogue();
                UIManager.Instance.HideOptions();

                CompleteElement();
            }
            else if (_listenForOptions)
            {
                if (Input.GetKey(KeyCode.Alpha1))
                    ChooseOption(0);
                else if (Input.GetKey(KeyCode.Alpha2))
                    ChooseOption(1);
                else if (Input.GetKey(KeyCode.Alpha3))
                    ChooseOption(2);
                else if (Input.GetKey(KeyCode.Alpha4))
                    ChooseOption(3);
            }
            
        }

        private void ChooseOption(int selection)
        {
            if (selection < _activeOptions.Count && _activeOptions[selection].Outcome != null)
            {
                _listenForOptions = false;
                StartInteraction(_activeOptions[selection].Outcome);
                
                // temp here
                UIManager.Instance.HideOptions();
                return;
            }
            Debug.LogError("Invalid option!");
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
                CameraManager.Instance.ResetCamera();
                UIManager.Instance.ShowEquipment();
                _interaction = null;

                if (_interactable != null)
                {
                    UIManager.Instance.ShowInteractableText(_interactable.Text);
                }
            }
        }

        // todo: move
        public void ListenForKey(KeyCode keyCode = KeyCode.Escape)
        {
            _listenForKey = true;
            _keyCode = keyCode;
        }

        public void ListenForOptions(List<Option> options)
        {
            _listenForOptions = true;
            _activeOptions = options;
        }
    }
}