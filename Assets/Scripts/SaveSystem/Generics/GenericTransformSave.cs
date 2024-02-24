using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace SaveSystem.Generics
{
    [Serializable]
    public struct SerializeVector3
    {
        public float x;
        public float y;
        public float z;

        public SerializeVector3(Vector3 vector3)
        {
            x = vector3.x;
            y = vector3.y;
            z = vector3.z;
        }

        public Vector3 ToVector3() => new(x, y, z);
    }
    
    public class GenericTransformSave : MonoBehaviour, ISavable
    {
        [SerializeField] private string _guid;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_guid))
                _guid = Guid.NewGuid().ToString();
        }

        private Dictionary<string, object> GetDataToSave()
        {
            var objectTransform = transform;
        
            var data = new Dictionary<string, object>
            {
                {"position", new SerializeVector3(objectTransform.position)},
                {"rotation", new SerializeVector3(objectTransform.rotation.eulerAngles)},
                {"scale", new SerializeVector3(objectTransform.localScale)}
            };

            return data;
        }
    
        #region ISavable

        public string Key => _guid;
        public object DefaultValue => GetDataToSave();
        public int Priority => 0;
    
        public object Save()
        {
            return GetDataToSave();
        }

        public void Load(object value)
        {
            var data = (Dictionary<string, object>)value;

            var objectTransform = transform;
            
            objectTransform.position = ((SerializeVector3)data["position"]).ToVector3();
            objectTransform.rotation = Quaternion.Euler(((SerializeVector3)data["rotation"]).ToVector3());
            objectTransform.localScale = ((SerializeVector3)data["scale"]).ToVector3();
        }

        #endregion
    }
}
