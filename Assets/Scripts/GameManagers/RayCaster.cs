using System;
using System.Collections.Generic;
using System.Linq;
using ToolTipSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

namespace GameManagers
{
    public class RayCaster : MonoBehaviour
    {
        #region Events

        /// <summary>
        /// Invoked when the GameObject that is hit first by a raycast from the camera center to the player changes.
        /// </summary>
        public event Action<GameObject> OnLookingAtPlayerFromCameraCenterChange;

        /// <summary>
        /// Invoked when the GameObject that is hit first by a ray cast from the camera center to the player changes.
        /// </summary>
        public event Action<GameObject[]> OnLookingAtPlayerFromCameraCenterChangeArray;

        /// <summary>
        /// Invoked when the GameObject that is behind the mouse cursor changes.
        /// </summary>
        public event Action<GameObject> OnLookingAtObjectBehindTheMouseChange;

        #endregion

        #region Public properties

        /// <summary>
        /// Returns the GameObject that is hit first by a raycast from the camera center to the player.
        /// </summary>
        public GameObject LookingAtPlayerFromCameraCenter
        {
            get => _lookingAtPlayerFromCameraCenter;
            private set
            {
                if (_lookingAtPlayerFromCameraCenter == value) return;
                _lookingAtPlayerFromCameraCenter = value;
                OnLookingAtPlayerFromCameraCenterChange?.Invoke(_lookingAtPlayerFromCameraCenter);
            }
        }
        
        /// <summary>
        /// Returns the array of GameObjects that is hit first by a raycast from the camera center to the player.
        /// </summary>
        public GameObject[] LookingAtPlayerFromCameraCenterArray
        {
            get => _lookingAtPlayerFromCameraCenterArray;
            private set
            {
                if (_lookingAtPlayerFromCameraCenterArray == value) return;
                _lookingAtPlayerFromCameraCenterArray = value;
                OnLookingAtPlayerFromCameraCenterChangeArray?.Invoke(_lookingAtPlayerFromCameraCenterArray);
            }
        }
    
        /// <summary>
        /// Returns the GameObject that is behind the mouse cursor.
        /// </summary>
        public GameObject LookingAtObjectBehindTheMouse
        {
            get => _lookingAtObjectBehindTheMouse;
            private set
            {
                if (_lookingAtObjectBehindTheMouse == value) return;
                _lookingAtObjectBehindTheMouse = value;
                OnLookingAtObjectBehindTheMouseChange?.Invoke(_lookingAtObjectBehindTheMouse);
            }
        }

        /// <summary>
        /// Returns the RaycastHit that is hit first by a raycast from the camera center to the player.
        /// </summary>
        public RaycastHit LookingAtPlayerFromCameraCenterRayCastHit { get; private set; }
        
        /// <summary>
        /// Returns the array of RaycastHit that is hit first by a raycast from the camera center to the player.
        /// </summary>
        public RaycastHit[] LookingAtPlayerFromCameraCenterArrayRayCastHit { get; private set; }
    
        /// <summary>
        /// Returns the RaycastHit that is behind the mouse cursor.
        /// </summary>
        public RaycastHit LookingAtObjectBehindTheMouseRayCastHit { get; private set; }

        #endregion

        #region Private fields

        [InjectOptional] private PlayerSystem.Player _player;
        [Inject] private Camera _camera;
        
        private GameObject _lookingAtPlayerFromCameraCenter;
        private GameObject[] _lookingAtPlayerFromCameraCenterArray;
        private GameObject _lookingAtObjectBehindTheMouse;

        #endregion

        public void Update()
        {
            UpdateLookingAtPlayerFromCameraCenter();
            UpdateLookingAtPlayerFromCameraCenterArray();
            UpdateLookingAtObjectBehindTheMouse();
        }

        private void UpdateLookingAtPlayerFromCameraCenter()
        {
            if (_player == null) return;
            
            var origin = _camera.transform.position;
            var direction = _player.transform.position - origin;
            var maxDistance = direction.magnitude;
            var ray = new Ray(origin, direction);

            Physics.Raycast(ray, out var hit, maxDistance);

            LookingAtPlayerFromCameraCenterRayCastHit = hit;
            LookingAtPlayerFromCameraCenter = hit.transform == null ? null : hit.transform.gameObject;
        }
        
        private void UpdateLookingAtPlayerFromCameraCenterArray()
        {
            if (_player == null) return;
            
            var origin = _camera.transform.position;
            var direction = _player.transform.position - origin;
            var maxDistance = direction.magnitude;
            var ray = new Ray(origin, direction);

            var hit = new RaycastHit[] { };
            Physics.SphereCastNonAlloc(ray, 0.5f, hit, maxDistance);

            LookingAtPlayerFromCameraCenterArrayRayCastHit = hit;
            LookingAtPlayerFromCameraCenterArray = hit.Select(x => x.transform.gameObject).ToArray();
        }
    
        private void UpdateLookingAtObjectBehindTheMouse()
        {
            var hitWorldSpace = new List<GameObject>();
            var hitScreenSpace = new List<GameObject>();
            
            // Get hit objects from world space
            var ray = _camera.ScreenPointToRay(Mouse.current.position.value);
            Physics.Raycast(ray, out var hit, Mathf.Infinity);
            
            hitWorldSpace.Add(hit.transform == null ? null : hit.transform.gameObject);
            
            
            // Get hit objects from UI
            var pointer = new PointerEventData(EventSystem.current)
            {
                position = Mouse.current.position.value
            };
            
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, results);

            hitScreenSpace.AddRange(results.Where(result => result.gameObject is not null)
                .Select(result => result.gameObject));
            
            
            // Set
            LookingAtObjectBehindTheMouseRayCastHit = hit;
            
            LookingAtObjectBehindTheMouse = hitScreenSpace.Count > 0
                ? hitScreenSpace.FirstOrDefault(go => go is not null)
                : hitWorldSpace.FirstOrDefault(go => go is not null);
        }
    }
}
