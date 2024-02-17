using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace ToolTipSystem
{
    public class UIToolTipScreenSpace : ToolTipDisplay
    {
        [SerializeField] private int _offset;
        
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Image _icon;
        [SerializeField] private RectTransform _rectTransform;
        
        [Inject] private Camera _camera;

        public override void UpdateUI(ToolTipData data)
        {
            _title.text = data.Title;
            _description.text = data.Description;
            _icon.sprite = data.Icon;
        }

        public override void HandleToolTipPosition()
        {
            var toolTipGameObject = (CurrentToolTip as MonoBehaviour)?.gameObject;
            var mousePosition = Mouse.current.position.value;
            
            if (toolTipGameObject is null) return;
            
            var screenSize = new Vector2(Screen.width, Screen.height);
            var screenCenter = screenSize / 2;
            
            var newPivotPosition = Vector2.zero;
            newPivotPosition.x = mousePosition.x > screenCenter.x ? 1 : 0;
            newPivotPosition.y = mousePosition.y > screenCenter.y ? 1 : 0;
            _rectTransform.pivot = newPivotPosition;

            var pivotOffset = new Vector2(_rectTransform.pivot.x == 0 ? _offset : -_offset,
                _rectTransform.pivot.y == 0 ? _offset : -_offset);
            
            _rectTransform.position = mousePosition + pivotOffset;
        }
    }
}