using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace GameInputSystem
{
    public class Input : MonoBehaviour
    {
        [Inject] private InputManager _inputManager;

        public PlayerController PlayerController => new(GetActionMap("PlayerController"));
        public CameraController CameraController => new(GetActionMap("CameraController"));
        public Interaction Interaction => new(GetActionMap("Interaction"));
        public Mouse Mouse => new(GetActionMap("Mouse"));
        
        private InputActionMap GetActionMap(string actionMapName)
        {
            return _inputManager.InputActionAsset.FindActionMap(actionMapName);
        }
    }
    
    public class PlayerController
    {
        public InputActionMap Map { get; private set; }
        
        public PlayerController(InputActionMap map)
        {
            Map = map;
        }
        
        public InputAction Move => Map.FindAction("Move");
        public InputAction Sneak => Map.FindAction("Sneak");
    }
    
    public class CameraController
    {
        public InputActionMap Map { get; private set; }
        
        public CameraController(InputActionMap map)
        {
            Map = map;
        }
        
        public InputAction Move => Map.FindAction("Move");
        public InputAction Rotation => Map.FindAction("Rotation");
        public InputAction FindPlayer => Map.FindAction("FindPlayer");
        public InputAction Zoom => Map.FindAction("Zoom");
    }

    public class Interaction
    {
        public InputActionMap Map { get; private set; }
        
        public Interaction(InputActionMap map)
        {
            Map = map;
        }
        
        public InputAction Interact => Map.FindAction("Interact");
    }

    public class Mouse
    {
        public InputActionMap Map { get; private set; }
        
        public Mouse(InputActionMap map)
        {
            Map = map;
        }
        
        public InputAction Position => Map.FindAction("Position");
    }
}