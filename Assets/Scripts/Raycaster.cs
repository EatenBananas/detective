using System;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    public static Raycaster Instance { get; set; }
    
    public Action<GameObject> OnLookAtChange { get; set; }
    
    public GameObject LookAt
    {
        get => _lookAt;
        private set
        {
            if (value == _lookAt) return;
            _lookAt = value;
            OnLookAtChange.Invoke(_lookAt);
        }
    }

    private GameObject _lookAt;
    private Ray _ray;
    private RaycastHit _rayHit;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Update()
    {
        var origin = Getter.Instance.MainCamera.transform.position;
        var direction = Getter.Instance.Player.transform.position - origin;
        _ray = new Ray(origin, direction);

        Physics.Raycast(_ray, out _rayHit, Mathf.Infinity);

        LookAt = _rayHit.transform != null ? _rayHit.transform.gameObject : null;
    }
}
