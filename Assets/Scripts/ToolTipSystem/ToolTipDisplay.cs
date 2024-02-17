using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace ToolTipSystem
{
    public abstract class ToolTipDisplay : MonoBehaviour
    {
        public IToolTip CurrentToolTip
        {
            get => _currentToolTip;
            set
            {
                if (_currentToolTip == value) return;
                
                _currentToolTip = value;

                Hide();
                
                if (_currentToolTip is null) return;
                
                UpdateUI(_currentToolTip.ToolTipData);

                Show();
            }
        }
        
        private IToolTip _currentToolTip;
        private const float ANIM_DURATION = 0.25f;
        private string _animId;
        
        public void Update()
        {
            if (CurrentToolTip is null) return;
            
            HandleToolTipPosition();
        }

        public abstract void UpdateUI(ToolTipData data);
        public abstract void HandleToolTipPosition();
        
        private void Show()
        {
            DOTween.Kill(_animId);
            
            _animId = Guid.NewGuid().ToString();
            
            transform.DOScale(Vector3.one, ANIM_DURATION)
                .SetId(_animId)
                .SetUpdate(true)
                .SetEase(Ease.OutBack)
                .onPlay += () => gameObject.SetActive(true);
        }

        private void Hide()
        {
            DOTween.Kill(_animId);
            
            _animId = Guid.NewGuid().ToString();
            
            transform.DOScale(Vector3.zero, ANIM_DURATION)
                .SetId(_animId)
                .SetUpdate(true)
                .SetEase(Ease.InBack)
                .onComplete += () => gameObject.SetActive(false);
        }
    }
}