using System;
using UnityEngine;

namespace SceneObjects
{
    public class SceneObject : MonoBehaviour
    {
        [SerializeField] private SceneReference _reference;

        private void Start()
        {
            if (_reference == null)
            {
                return;
            }
            
            SceneObjectManager.Instance.Register(_reference, gameObject);
        }
    }
}
