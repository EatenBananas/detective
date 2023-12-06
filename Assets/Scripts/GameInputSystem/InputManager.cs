using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using Zenject;

namespace GameInputSystem
{
    public class InputManager : MonoBehaviour
    {
        #region Events

        public static Action<InputActionMap> OnInputActionMapEnabled { get; private set; }
        public static Action<InputActionMap> OnInputActionMapDisabled { get; private set; }
        public static Action<InputActionMap> OnInputActionEnabled { get; private set; }
        public static Action<InputActionMap> OnInputActionDisabled { get; private set; }

        #endregion

        [field: SerializeField] public InputActionAsset InputActionAsset { get; private set; }
        [field: SerializeField] public Input Input { get; private set; }

        [field: SerializeField, Header("Unity Player Input System")] public PlayerInputManager PlayerInputManager { get; private set; }
        [field: SerializeField] public PlayerInput PlayerInput { get; private set; }

        [field: SerializeField, Header("Unity Event System")] public EventSystem EventSystem { get; private set; }
        [field: SerializeField] public InputSystemUIInputModule InputSystemUIInputModule { get; private set; }
        
        [Inject] private Camera _camera;

        private void Awake()
        {
            PlayerInput.camera = _camera;
        }
    }

    public static class InputManagerExtensions
    {
        public static void EnableInputActionMap(this InputActionMap inputActionMap)
        {
            if (inputActionMap.enabled) return;
            inputActionMap.Enable();
            InputManager.OnInputActionMapEnabled?.Invoke(inputActionMap);
        }
        
        public static void DisableInputActionMap(this InputActionMap inputActionMap)
        {
            if (!inputActionMap.enabled) return;
            inputActionMap.Disable();
            InputManager.OnInputActionMapDisabled?.Invoke(inputActionMap);
        }
        
        public static void EnableInputAction(this InputAction inputAction)
        {
            if (inputAction.enabled) return;
            inputAction.Enable();
            InputManager.OnInputActionEnabled?.Invoke(inputAction.actionMap);
        }
        
        public static void DisableInputAction(this InputAction inputAction)
        {
            if (!inputAction.enabled) return;
            inputAction.Disable();
            InputManager.OnInputActionDisabled?.Invoke(inputAction.actionMap);
        }
    }
}
