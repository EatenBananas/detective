using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace InputSystem
{
    public class Input : MonoBehaviour
    {
        [Inject] private InputManager _inputManager;

        public Player Player => new(GetActionMap("Player"));
        public Camera Camera => new(GetActionMap("Camera"));
        public Interaction Interaction => new(GetActionMap("Interaction"));
        public Mouse Mouse => new(GetActionMap("Mouse"));
        
        private InputActionMap GetActionMap(string actionMapName)
        {
            return _inputManager.InputActionAsset.FindActionMap(actionMapName);
        }
    }
    
    public class Player
    {
        public InputActionMap Map { get; private set; }
        
        public Player(InputActionMap map)
        {
            Map = map;
        }
        
        public InputAction Move => Map.FindAction("Move");
        public InputAction Sneak => Map.FindAction("Sneak");
    }
    
    public class Camera
    {
        public InputActionMap Map { get; private set; }
        
        public Camera(InputActionMap map)
        {
            Map = map;
        }
        
        public InputAction Move => Map.FindAction("Move");
        public InputAction Rotation => Map.FindAction("Rotation");
        public InputAction FindPlayer => Map.FindAction("FindPlayer");
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