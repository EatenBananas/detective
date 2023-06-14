using System;
using System.Collections.Generic;
using System.Linq;
using Interactions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneObjects
{
    public class SceneObjectManager : MonoBehaviour
    {
        public static SceneObjectManager Instance { get; private set; }
        public Dictionary<SceneReference, Vector3> SceneLocations { get; private set; } = new();

        // temp shit
        [field:SerializeField] private GameObject _player;
        
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
                    SceneLocations.Add(key, sceneObject.transform.position);
                    break;
                }
                default:
                {
                    Debug.LogError("Unknown Scene Reference Type");
                    break;
                }
            }
        }

        // temp here
        public void Teleport(SceneReference location)
        {
            if (!SceneLocations.ContainsKey(location))
            {
                Debug.LogError("Scene Location not found");
                return;
            }

            _player.transform.position = SceneLocations[location];
        }
        
    }
}
