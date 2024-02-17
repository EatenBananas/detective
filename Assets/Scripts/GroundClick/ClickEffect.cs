using PlayerSystem;
using UnityEngine;
using Zenject;

namespace GroundClick
{
    public class ClickEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _clickEffectPrefab;
        
        [Inject] private Player _player;

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
            Instantiate(_clickEffectPrefab, hit.point, Quaternion.identity);
        }
    }
}
