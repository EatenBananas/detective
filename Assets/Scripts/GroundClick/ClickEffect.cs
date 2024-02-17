using ObjectPoolingSystem;
using PlayerSystem;
using UnityEngine;
using Zenject;

namespace GroundClick
{
    public class ClickEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _clickEffectPrefab;
        
        [Inject] private Player _player;
        [Inject] private ObjectPoolingManager _objectPoolingManager;

        private void OnEnable()
        {
            _player.Input.OnMove += OnPlayerMove;
        }
        
        private void OnDisable()
        {
            _player.Input.OnMove -= OnPlayerMove;
        }
        
        private void OnPlayerMove(RaycastHit hit)
        {
            var clickEffect = _objectPoolingManager.GetFromPool(_clickEffectPrefab.gameObject);
            clickEffect.transform.position = hit.point;
            clickEffect.SetActive(true);
        }
    }
}
