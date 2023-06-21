using System;
using System.Collections.Generic;
using System.Linq;
using Interactions;
using Player.Movement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneObjects
{
    public class SceneObjectManager : MonoBehaviour
    {
        public static SceneObjectManager Instance { get; private set; }
        public Dictionary<SceneReference, Vector3> SceneLocations { get; private set; } = new();

        // temp shit
        [field: SerializeField] private PlayerMovement _playerMovement;
        
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

            _playerMovement.TeleportPlayer(SceneLocations[location]);
        }
        
    }
}
