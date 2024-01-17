using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace GameManagers
{
    public class RayCaster : MonoBehaviour
    {
        #region Events

        /// <summary>
        /// Invoked when the GameObject that is hit first by a raycast from the camera center to the player changes.
        /// </summary>
        public Action<GameObject> OnLookingAtPlayerFromCameraCenterChange { get; set; }
        
        /// <summary>
        /// Invoked when the GameObject that is hit first by a ray cast from the camera center to the player changes.
        /// </summary>
        public Action<GameObject[]> OnLookingAtPlayerFromCameraCenterChangeArray { get; set; }
    
        /// <summary>
        /// Invoked when the GameObject that is behind the mouse cursor changes.
        /// </summary>
        public Action<GameObject> OnLookingAtObjectBehindTheMouseChange { get; set; }

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

        [Inject] private PlayerSystem.Player _player;
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
            var origin = _camera.transform.position;
            var direction = _player.transform.position - origin;
            var maxDistance = direction.magnitude;
            var ray = new Ray(origin, direction);

            Physics.Raycast(ray, out var hit, maxDistance);

            LookingAtPlayerFromCameraCenterRayCastHit = hit;
            
            if (hit.transform is not null) 
                LookingAtPlayerFromCameraCenter = hit.transform.gameObject;
        }
        
        private void UpdateLookingAtPlayerFromCameraCenterArray()
        {
            var origin = _camera.transform.position;
            var direction = _player.transform.position - origin;
            var maxDistance = direction.magnitude;
            var ray = new Ray(origin, direction);

            var hit = new RaycastHit[] { };
            Physics.SphereCastNonAlloc(ray, 0.5f, hit, maxDistance);

            LookingAtPlayerFromCameraCenterArrayRayCastHit = hit;
            if (hit.Length != 0)
                LookingAtPlayerFromCameraCenterArray = hit.Select(x => x.transform.gameObject).ToArray();
        }
    
        private void UpdateLookingAtObjectBehindTheMouse()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
        
            Physics.Raycast(ray, out var hit, Mathf.Infinity);
            
            LookingAtObjectBehindTheMouseRayCastHit = hit;
            
            if (hit.transform is not null) 
                LookingAtObjectBehindTheMouse = hit.transform.gameObject;
        }
    }
}
