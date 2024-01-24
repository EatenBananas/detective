using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interactions;
using Unity.VisualScripting;
// using Player.Movement;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace SceneObjects
{
    public class SceneObjectManager : MonoBehaviour
    {
        public static SceneObjectManager Instance { get; private set; }
        private Dictionary<SceneReference, Vector3> _sceneLocations = new();
        private Dictionary<SceneReference, GameObject> _photos = new();
        private Dictionary<SceneReference, PlayableDirector> _cutscenes = new();

        // TODO: add method for teleport player
        // temp shit
        // [field: SerializeField] private PlayerMovement _playerMovement;
        
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
                    _sceneLocations.Add(key, sceneObject.transform.position);
                    break;
                }
                case SceneReferenceType.PHOTO:
                {
                    _photos.Add(key, sceneObject.gameObject);
                    sceneObject.SetActive(false);
                    break;
                }
                case SceneReferenceType.CUTSCENE:
                {
                    _cutscenes.Add(key, sceneObject.gameObject.GetComponent<PlayableDirector>());
                    sceneObject.gameObject.SetActive(false);
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
            if (!_sceneLocations.ContainsKey(location))
            {
                Debug.LogError("Scene Location not found");
                return;
            }

            // _playerMovement.TeleportPlayer(_sceneLocations[location]);
        }

        public void UpdatePhoto(SceneReference photo, bool visible)
        {
            if (!_photos.ContainsKey(photo))
            {
                Debug.LogError("Scene Photo not found");
                return;
            }
            
            _photos[photo].SetActive(visible);
        }

        public void PlayCutscene(SceneReference cutscene)
        {
            var clip = _cutscenes[cutscene];
            clip.gameObject.SetActive(true);
            clip.Play();
            clip.stopped += director => InteractionManager.Instance.CompleteElement();
            clip.stopped += director => clip.gameObject.SetActive(false);
        }
    }
}
