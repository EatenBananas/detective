using UnityEngine;

namespace CutoutObjectSystem
{
    public class CutoutObjectManager : MonoBehaviour
    {
        [SerializeField] private LayerMask _wallLayerMask;
        
        private Renderer _lastRenderer;

        private void OnEnable()
        {
            Raycaster.Instance.OnLookAtChange += HideWall;
        }

        private void OnDisable()
        {
            Raycaster.Instance.OnLookAtChange -= HideWall;
        }

        private void HideWall(GameObject wall)
        {
            if (wall == null)
            {
                if (_lastRenderer != null) _lastRenderer.enabled = true;
                _lastRenderer = null;
                return;
            }
            
            if (wall.layer == 7)
            {
                if (wall.TryGetComponent(out Renderer renderer))
                {
                    if (_lastRenderer != null) _lastRenderer.enabled = true;

                    renderer.enabled = false;

                    _lastRenderer = renderer;
                }
                
                return;
            }
            
            if (_lastRenderer != null) _lastRenderer.enabled = true;
            _lastRenderer = null;
        }
    }
}