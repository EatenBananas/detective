using System;
using GameManagers;
using UnityEngine;
using Zenject;

namespace InfoSystem
{
    public class InfoManager : MonoBehaviour
    {
        public IInfo CurrentInfo
        {
            get => _currentInfo;
            set
            {
                if (_currentInfo == value) return;
                _currentInfo = value;
                if (_currentInfo is null) HideInfo();
                else ShowInfo(_currentInfo);
            }
        }

        [SerializeField] private InfoPanel _infoPanel;
        
        [Inject] private RayCaster _rayCaster;
        
        private IInfo _currentInfo;

        private void OnEnable()
        {
            _rayCaster.OnLookingAtObjectBehindTheMouseChange += OnLookingAtObjectBehindTheMouseChange;
        }
        
        private void OnDisable()
        {
            _rayCaster.OnLookingAtObjectBehindTheMouseChange -= OnLookingAtObjectBehindTheMouseChange;
        }
        
        private void OnLookingAtObjectBehindTheMouseChange(GameObject obj)
        {
            if (obj is null)
            {
                CurrentInfo = null;
                return;
            }

            CurrentInfo = obj.TryGetComponent(out IInfo info) ? info : null;
        }
        
        private void ShowInfo(IInfo info)
        {
            _infoPanel.UpdateInfo(info);
            _infoPanel.ShowInfo();
        }
        
        private void HideInfo() => _infoPanel.HideInfo();
    }
}
