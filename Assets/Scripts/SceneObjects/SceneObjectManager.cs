using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SceneObjects
{
    public class SceneObjectManager : MonoBehaviour
    {
        public static SceneObjectManager Instance { get; private set; }
        public Dictionary<SceneReference, Transform> SceneLocations { get; private set; } = new();

        private void Awake()
        {
            Instance = this;
        }

        public void Register(SceneReference key, GameObject sceneObject)
        {
            switch (key.SceneObjectType)
            {
                case SceneReferenceType.CAMERA:
                {
                    CameraManager.Instance.Register(key, sceneObject);
                    break;
                }
                case SceneReferenceType.LOCATION:
                {
                    SceneLocations.Add(key, sceneObject.transform);
                    break;
                }
                default:
                {
                    Debug.LogError("Unknown Scene Reference Type");
                    break;
                }
            }
        }
        
    }
}
