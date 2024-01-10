using System;
using DG.Tweening;
using GameInputSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace InfoSystem
{
    public class InfoPanel : MonoBehaviour
    {
        [SerializeField] private float _animDuration;
        
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Image _icon;
        
        [Inject] private Camera _camera;
        
        private Vector3 _initialScale;

        private void Awake() => _initialScale = transform.localScale;

        public void UpdateInfo(IInfo info)
        {
            _title.text = info.GetTitle();
            _description.text = info.GetDescription();
            _icon.sprite = info.GetIcon();
        }

        public void ShowInfo()
        {
            transform.DOScale(_initialScale, _animDuration)
                .SetUpdate(true)
                .SetEase(Ease.OutBack)
                .onPlay += () => gameObject.SetActive(true);
        }

        public void HideInfo()
        {
            transform.DOScale(Vector3.zero, _animDuration)
                .SetUpdate(true)
                .SetEase(Ease.InBack)
                .onComplete += () => gameObject.SetActive(false);
        }

        public void Update()
        {
            if (!gameObject.activeSelf) return;
            
            var infoTransform = transform;
            var cameraTransform = _camera.transform;
            var cameraPosition = cameraTransform.position;
                
            infoTransform.LookAt(cameraPosition, Vector3.down);
            infoTransform.position = cameraPosition + cameraTransform.forward * 2;
        }
    }
}