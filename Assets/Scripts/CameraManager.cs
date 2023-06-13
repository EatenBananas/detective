using System;
using System.Collections.Generic;
using System.Linq;
using SceneObjects;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    private SceneReference _activeCamera;
    private readonly Dictionary<SceneReference, Camera> _cameras = new();

    private void Awake()
    {
        Instance = this;
    }

    public void Register(SceneReference reference, GameObject sceneObject)
    {
        var cameraComponent = sceneObject.GetComponent<Camera>();
        if (cameraComponent == null)
        {
            Debug.LogError("Camera Component not found");
            return;
        }

        _cameras[reference] = cameraComponent;
    }

    private void Start()
    {
        RefreshCameras();
    }

    public void ChangeCamera(SceneReference sceneCamera)
    {
        if (!_cameras.ContainsKey(sceneCamera))
        {
            Debug.LogError("Camera id not found");
            return;
        }
        
        _activeCamera = sceneCamera;
        RefreshCameras();
    }

    private void RefreshCameras()
    {
        foreach (var camera1 in _cameras)
        {
            camera1.Value.enabled = (camera1.Key == _activeCamera);
        }
    }
    
    public void ResetCamera()
    {
        _activeCamera = null;
        RefreshCameras();
    }
    
}