using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ToolTipSystem
{
    public class UIToolTipWorldSpace : ToolTipDisplay
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Image _icon;
        
        [Inject] private Camera _camera;

        public override void UpdateUI(ToolTipData data)
        {
            _title.text = data.Title;
            _description.text = data.Description;
            _icon.sprite = data.Icon;
        }

        public override void HandleToolTipPosition()
        {
            var uiTransform = transform;
            var toolTipGameObject = (CurrentToolTip as MonoBehaviour)?.gameObject;
            
            if (toolTipGameObject is null) return;
            
            uiTransform.position = toolTipGameObject.transform.position + Vector3.up * 2;
            uiTransform.forward = _camera.transform.forward;
        }
    }
}