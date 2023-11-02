using Player.Movement;
using UnityEngine;

public class Getter : MonoBehaviour
{
    public static Getter Instance { get; set; }
    
    public Camera MainCamera
    {
        get
        {
            if (_mainCamera == null) _mainCamera = Camera.main;
            return _mainCamera;
        }
    }
    public Transform Player { get; private set; }

    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;

        if (TryGetComponent(out PlayerMovement playerMovement)) 
            Player = playerMovement.transform;

        if (Instance == null) Instance = this;
    }
}