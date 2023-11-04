using System;
using UnityEngine;
using Zenject;

namespace GameManagers
{
    public class Raycaster : ITickable
    {
        #region Events

        /// <summary>
        /// Invoked when the GameObject that is hit first by a raycast from the camera center to the player changes.
        /// </summary>
        public Action<GameObject> OnLookingAtPlayerFromCameraCenterChange { get; set; }
    
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
                OnLookingAtPlayerFromCameraCenterChange.Invoke(_lookingAtPlayerFromCameraCenter);

                Debug.Log($"{nameof(LookingAtPlayerFromCameraCenter)}: {LookingAtPlayerFromCameraCenter} ");
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
                OnLookingAtObjectBehindTheMouseChange.Invoke(_lookingAtObjectBehindTheMouse);
                
                Debug.Log($"{nameof(LookingAtObjectBehindTheMouse)}: {LookingAtObjectBehindTheMouse} ");
            }
        }

        /// <summary>
        /// Returns the RaycastHit that is hit first by a raycast from the camera center to the player.
        /// </summary>
        public RaycastHit LookingAtPlayerFromCameraCenterRaycastHit { get; private set; }
    
        /// <summary>
        /// Returns the RaycastHit that is behind the mouse cursor.
        /// </summary>
        public RaycastHit LookingAtObjectBehindTheMouseRaycastHit { get; private set; }

        #endregion

        #region Private fields

        private GameObject _lookingAtPlayerFromCameraCenter;
        private GameObject _lookingAtObjectBehindTheMouse;

        #endregion

        public void Tick()
        {
            UpdateLookingAtPlayerFromCameraCenter();
            UpdateLookingAtObjectBehindTheMouse();
        }

        private void UpdateLookingAtPlayerFromCameraCenter()
        {
            var origin = Getter.Instance.MainCamera.transform.position;
            var direction = Getter.Instance.Player.transform.position - origin;
            var maxDistance = direction.magnitude;
            var ray = new Ray(origin, direction);

            Physics.Raycast(ray, out var hit, maxDistance);

            LookingAtPlayerFromCameraCenterRaycastHit = hit;
            if (hit.transform != null) LookingAtPlayerFromCameraCenter = hit.transform.gameObject;
        }
    
        private void UpdateLookingAtObjectBehindTheMouse()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
            Physics.Raycast(ray, out var hit, Mathf.Infinity);
            
            LookingAtObjectBehindTheMouseRaycastHit = hit;
            if (hit.transform != null) LookingAtObjectBehindTheMouse = hit.transform.gameObject;
        }
    }
}
