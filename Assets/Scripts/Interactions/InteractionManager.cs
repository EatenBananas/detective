using System;
using System.Collections;
using System.Collections.Generic;
using Interactions.Elements;
using PlayerSystem;
using TMPro;
using UnityEngine;

namespace Interactions
{
    public class InteractionManager : MonoBehaviour
    {
        // todo: replace with zenject
        public static InteractionManager Instance { get; private set; }

        [field: SerializeField] private Player _player;
        
        private Interactable _interactable;
        private InteractionElement _interaction;
        private int _currentElementIndex;

        private KeyCode _keyCode;
        private bool _listenForKey = false;

        private List<Option> _activeOptions;
        private bool _listenForOptions = false;

        [SerializeField] private StartInteraction _startInteraction;
        [SerializeField] private float _startDelay = 1f;
        
        private void Start()
        {
            Instance = this;
            StartCoroutine(LateStart());
        }

        private IEnumerator LateStart()
        {
            yield return new WaitForSeconds(_startDelay);
            if (_startInteraction != null)
            {
                StartInteraction(_startInteraction);
            }
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
                    Debug.Log($"Entering {_interactable.name}");
                    Debug.Log($"Locking player...");
                    _player.LockPlayer(true);
                    UIManager.Instance.ShowPieMenu(_interactable.Interactions);
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
            Debug.Log("Option skip");
            CompleteElement();
        }

        public void StartInteraction(InteractionElement interaction)
        {
            _interaction = interaction;
            Debug.Log($"Starting {_interaction.name}");
            UIManager.Instance.HideInteractableText();
            UIManager.Instance.HideEquipment();
            //_currentElementIndex = 0;
            StartElement();
        }

        private void StartElement()
        {
            var element = _interaction;
            Debug.Log($"Starting {element.GetType()}");
            element.Execute();
        }

        public void CompleteElement()
        {
            Debug.Log("Completed");
            _currentElementIndex++;

            if (_interaction.NextElement != null)
            {
                _interaction = _interaction.NextElement;
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
                
                Debug.Log("Unlocking player...");
                _player.UnlockPlayer();
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