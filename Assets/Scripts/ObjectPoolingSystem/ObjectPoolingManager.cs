using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ObjectPoolingSystem
{
    public class ObjectPoolingManager : SerializedMonoBehaviour
    {
        [ShowInInspector] public Dictionary<GameObject, int> PoolStructure { get; private set; } = new();
        [ShowInInspector] public Dictionary<GameObject, List<ObjectClone>> PooledObjects { get; private set; } = new();
        
        [Button]
        private void ResetPool()
        {
            DeInitPool();
            InitPool();
        }
        
        private void InitPool()
        {
            foreach (var pooledObject in PoolStructure)
            {
                var objects =  CreateObjects(pooledObject.Key, pooledObject.Value, transform);
                
                PooledObjects.Add(pooledObject.Key, objects);
            }
        }

        private void DeInitPool()
        {
            PooledObjects.Clear();
            
            foreach (Transform t in transform) 
                DestroyImmediate(t.gameObject);
        }

        private static List<ObjectClone> CreateObjects(GameObject prefab, int amount, Transform parent)
        {
            var objects = new List<ObjectClone>();
            
            for (var i = 0; i < amount; i++)
            {
                var clone = CreateObject(prefab, parent);
                objects.Add(clone.GetComponent<ObjectClone>());
            }
            
            return objects;
        }
        
        private static ObjectClone CreateObject(GameObject prefab, Transform parent)
        {
            var clone = Instantiate(prefab, parent);
            clone.SetActive(false);

            if (!clone.TryGetComponent(out ObjectClone _)) 
                clone.AddComponent<ObjectClone>();

            return clone.GetComponent<ObjectClone>();
        }

        public void ReturnToPool<T>(T obj) where T : MonoBehaviour
        {
            if (!PooledObjects.ContainsKey(obj.gameObject)) return;
            if (!obj.TryGetComponent(out ObjectClone clone)) return;
            if (clone.IsInPool) return;
            
            clone.transform.SetParent(transform);

            if (!clone.gameObject.activeSelf)
                clone.gameObject.SetActive(false);
        }
        
        public T GetFromPool<T>(T prefab) where T : MonoBehaviour
        {
            var lookingFor = prefab.gameObject;
            
            if (!PooledObjects.ContainsKey(lookingFor)) return null;
            
            var findObjects = PooledObjects[lookingFor];
            
            foreach (var obj in findObjects)
            {
                if (obj.gameObject.activeSelf) continue;
                
                obj.gameObject.SetActive(true);

                return obj.GetComponent<T>();
            }

            var newObject = CreateObject(lookingFor, transform);
            
            PooledObjects[lookingFor].Add(newObject.GetComponent<ObjectClone>());
            
            return newObject.GetComponent<T>();
        }
    }
}
