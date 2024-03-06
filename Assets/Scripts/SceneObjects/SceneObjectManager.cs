using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interactions;
using PlayerSystem;
using Sirenix.OdinInspector;
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
        private Dictionary<SceneReference, Transform> _sceneLocations = new();
        private Dictionary<SceneReference, GameObject> _photos = new();
        private Dictionary<SceneReference, PlayableDirector> _cutscenes = new();
        
        [field: SerializeField] private Player _playerMovement;
        
        private PlayableDirector _activeCutscene;
        
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
                    _sceneLocations.Add(key, sceneObject.transform);
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

            _playerMovement.Teleport(_sceneLocations[location].position, true);
            _playerMovement.SetRotation(_sceneLocations[location].rotation);
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
            if (_activeCutscene != null)
            {
                _activeCutscene.Stop();
                _activeCutscene.gameObject.SetActive(false);
                _activeCutscene = null;
            }
            
            var clip = _cutscenes[cutscene];
            clip.gameObject.SetActive(true);

            foreach (Transform child in clip.gameObject.transform)
            {
                child.gameObject.SetActive(true);
            }

            clip.Play();

            if (clip.extrapolationMode == DirectorWrapMode.Loop)
            {
                _activeCutscene = clip;
                InteractionManager.Instance.CompleteElement();
            }
            else
            {
                clip.stopped += director => clip.gameObject.SetActive(false);
                clip.stopped += director => InteractionManager.Instance.CompleteElement();
            }
        }

        public void PlayAnim(string animName)
        {
            Debug.Log(animName);
            _playerMovement.Movement.Animator.Play(animName);
            StartCoroutine(AnimLoop(_playerMovement.Movement.Animator, animName));
            
        }

        private IEnumerator AnimLoop(Animator animator, string animName)
        {
            Debug.Log(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
            while (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == (animName))
            {
                yield return null;
            }
            Debug.Log(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
            
            //InteractionManager.Instance.CompleteElement();
        }
        
    }
}
