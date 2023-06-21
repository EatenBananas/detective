using Player.Movement;
using UnityEngine;

public class Getter : MonoBehaviour
{
    public static Camera MainCamera
    {
        get
        {
            if (_mainCamera == null) _mainCamera = Camera.main;
            return _mainCamera;
        }
    }
    public static Transform Player => _player;

    private static Camera _mainCamera;
    private static Transform _player;

    private void Awake()
    {
        TryGetComponent(out _mainCamera);

        if (TryGetComponent(out PlayerMovement playerMovement)) 
            _player = playerMovement.transform;
    }
}