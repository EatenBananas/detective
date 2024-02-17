using System;
using GameManagers;
using UnityEngine;
using Zenject;

namespace ToolTipSystem
{
    public class ToolTipManager : MonoBehaviour
    {
        [SerializeField] private UIToolTipWorldSpace _uiToolTipWorldSpace;
        [SerializeField] private UIToolTipScreenSpace _uiToolTipScreenSpace;
        
        [Inject] private RayCaster _rayCaster;

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
                SetToolTip(null);
                return;
            }

            obj.TryGetComponent(out IToolTip info);

            SetToolTip(info);
        }
        
        private void SetToolTip(IToolTip toolTip)
        {
            if (toolTip is null)
            {
                _uiToolTipScreenSpace.CurrentToolTip = null;
                _uiToolTipWorldSpace.CurrentToolTip = null;
                return;
            }
            
            switch (toolTip.ToolTipData.Type)
            {
                case ToolTipType.ScreenSpace:
                    _uiToolTipScreenSpace.CurrentToolTip = toolTip;
                    _uiToolTipWorldSpace.CurrentToolTip = null;
                    break;
                case ToolTipType.WorldSpace:
                    _uiToolTipScreenSpace.CurrentToolTip = null;
                    _uiToolTipWorldSpace.CurrentToolTip = toolTip;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
