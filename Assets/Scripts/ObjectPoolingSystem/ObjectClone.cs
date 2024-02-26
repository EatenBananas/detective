using UnityEngine;
using Zenject;

namespace ObjectPoolingSystem
{
    public class ObjectClone : MonoBehaviour
    {
        public bool IsInPool => transform.parent == _objectPoolingManager.transform;

        [Inject] private ObjectPoolingManager _objectPoolingManager;

        private void OnDisable()
        {
            if (!IsInPool)
                _objectPoolingManager.ReturnToPool(this);
        }
    }
}
