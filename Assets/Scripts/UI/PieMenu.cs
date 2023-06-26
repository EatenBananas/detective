using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PieMenu : MonoBehaviour
    {
        [SerializeField] private float _spacing = 0.05f;
        [SerializeField] private float _radius = 100f;
        [SerializeField] private List<PieMenuOption> _options = new();
        
        [SerializeField] private Button _addButton;
        [SerializeField] private Button _removeButton;

        [SerializeField] private GameObject _buttons;
        [SerializeField] private GameObject _icons;

        [SerializeField] private Image _tinyCircle;
        [SerializeField] private float _tinyCircleRadius = 60f;
        
        private void Start()
        {
            _addButton.onClick.AddListener(Add);
            _removeButton.onClick.AddListener(Remove);
        }

        private void Add()
        {
            _options.Add(new PieMenuOption()
            {
                DisplayText = (_options.Count + 1).ToString()
            });
            Reload();
        }

        private void Remove()
        {
            _options.RemoveAt(_options.Count - 1);
            Reload();
        }

        [ContextMenu("Reload")]
        private void Reload()
        {
            var buttons = _buttons.GetComponentsInChildren<PieMenuButton>(true);

            if (buttons.Length < _options.Count)
            {
                Debug.LogError("Pie menu: not enough buttons!");
            }

            var fill = 1f / _options.Count - _spacing;

            for (int i = 0; i < buttons.Length; i++)
            {
                if (i < _options.Count)
                {
                    buttons[i].Reload(fill, 360f / _options.Count * i);
                    buttons[i].gameObject.SetActive(true);
                    var text = _options[i].DisplayText;
                    buttons[i].Assign( ()=>Debug.Log($"Selected {text}") );
                }
                else
                {
                    buttons[i].gameObject.SetActive(false);
                }
            }

            var icons = _icons.GetComponentsInChildren<Image>(true);

            for (int i = 0; i < icons.Length; i++)
            {
                if (i < _options.Count)
                {
                    const float startOffset = 270f;
                    float angle = startOffset + 360f * (i - 0.5f) / _options.Count;
                    float angleInRadians = angle* (float)Math.PI / 180f;
                    
                    // Oblicz współrzędne x i y na obwodzie koła
                    float x = _radius * Mathf.Cos(angleInRadians);
                    float y = _radius * Mathf.Sin(angleInRadians);

                    // Ustaw pozycję obiektu na podstawie obliczonych współrzędnych
                    icons[i].rectTransform.localPosition   = new Vector3(x, y, 0f);
                    
                    icons[i].gameObject.SetActive(true);
                    icons[i].sprite = _options[i].Icon;
                }
                else
                {
                    icons[i].gameObject.SetActive(false);
                }
            }
        }

        private void OnValidate()
        {
            Reload();
        }

        private void Update()
        {
            var mousePos = Input.mousePosition;
            var angleInRadians = Mathf.Atan2(mousePos.y - Screen.height / 2f, mousePos.x - Screen.width / 2f);
            
            float x = _tinyCircleRadius * Mathf.Cos(angleInRadians);
            float y = _tinyCircleRadius * Mathf.Sin(angleInRadians);

            _tinyCircle.rectTransform.localPosition = new Vector3(x, y, 0f);
        }
    }
}
