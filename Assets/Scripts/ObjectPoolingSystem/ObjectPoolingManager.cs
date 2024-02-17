using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ObjectPoolingSystem
{
    public class ObjectPoolingManager : SerializedMonoBehaviour
    {
        [ShowInInspector] public Dictionary<GameObject, int> PoolStructure;
        [ShowInInspector] public Dictionary<GameObject, List<ObjectClone>> PooledObjects;
        
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
            
            for(var i = transform.childCount - 1; i >= 0; i--) 
                DestroyImmediate(transform.GetChild(i).gameObject);
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

        public void ReturnToPool(ObjectClone obj)
        {
            if (!PooledObjects.ContainsKey(obj.gameObject)) return;
            if (!obj.TryGetComponent(out ObjectClone clone)) return;
            if (clone.IsInPool) return;
            
            clone.transform.SetParent(transform);

            if (!clone.gameObject.activeSelf)
                clone.gameObject.SetActive(false);
        }
        
        public GameObject GetFromPool(GameObject prefab)
        {
            if (!PooledObjects.ContainsKey(prefab)) return null;
            
            var findObjects = PooledObjects[prefab];
            
            foreach (var obj in findObjects.Where(obj => !obj.gameObject.activeSelf))
                return obj.gameObject;

            var newObject = CreateObject(prefab, transform);
            
            PooledObjects[prefab].Add(newObject.GetComponent<ObjectClone>());

            return newObject.gameObject;
        }
    }
}
