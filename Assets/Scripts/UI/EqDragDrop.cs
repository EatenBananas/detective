using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class EqDragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
    {
        private RectTransform _rectTransform;
        // temp
        [SerializeField] private Canvas _canvas;
        [SerializeField] private int _slot = 0;
        private Vector3 _startPosition;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            _startPosition = _rectTransform.localPosition;
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }

        public void OnDrop(PointerEventData eventData)
        {
            UIManager.Instance.EqItemDropped(_slot);
            //_rectTransform.localPosition = _startPosition;
        }
    }
}
