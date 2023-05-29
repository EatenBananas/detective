using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    [SerializeField] private List<Camera> _cameras = new List<Camera>();
    private Camera _activeCamera;
        
    private void Start()
    {
        Debug.Log("Start");
        Instance = this;

        //_activeCamera = _cameras[0];
            
        foreach (var camera1 in _cameras)
        {
            camera1.gameObject.SetActive(camera1 == _activeCamera);
        }
    }

    public void ChangeCamera(int index)
    {
        if (index > _cameras.Count)
        {
            Debug.LogError("Camera id not found");
            return;
        }

        ResetCamera();
        
        _activeCamera = _cameras[index];
        _activeCamera.gameObject.SetActive(true);
    }

    public void ResetCamera()
    {
        if (_activeCamera == null) return;
        
        _activeCamera.gameObject.SetActive(false);
        _activeCamera = null;
    }

    public List<(string, int)> GetCameras()
    {
        return _cameras.Select((cam, i) => (cam.name, i)).ToList();
    }

    public string[] GetCameraNames() => _cameras.Select(cam => cam.name).ToArray();
    public int[] GetCameraIndexes() => _cameras.Select((cam, i) => i).ToArray();
}