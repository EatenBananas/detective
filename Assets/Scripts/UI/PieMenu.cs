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
        [SerializeField] private List<string> _options = new List<string>();

        [SerializeField] private Button _addButton;
        [SerializeField] private Button _removeButton;

        private void Start()
        {
            _addButton.onClick.AddListener(Add);
            _removeButton.onClick.AddListener(Remove);
        }

        private void Add()
        {
            _options.Add((_options.Count +1).ToString());
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
            var buttons = GetComponentsInChildren<PieMenuButton>(true);

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
                }
                else
                {
                    buttons[i].gameObject.SetActive(false);
                }
            }

            var texts = GetComponentsInChildren<TextMeshProUGUI>(true);

            for (int i = 0; i < texts.Length; i++)
            {
                if (i < _options.Count)
                {
                    
                    //_textField.rectTransform.eulerAngles = Vector3.zero;

                    // Konwertuj kąt na radiany
                    float angleInRadians = Mathf.PI * 2 / _options.Count * (i + 0.5f); //* (buttons - 1); //- Mathf.PI / buttons;
                    //Debug.Log($"{index}: {angleInRadians}");
            
                    // Oblicz współrzędne x i y na obwodzie koła
                    float x = _radius * Mathf.Cos(angleInRadians);
                    float y = _radius * Mathf.Sin(angleInRadians);

                    // Ustaw pozycję obiektu na podstawie obliczonych współrzędnych
                    texts[i].rectTransform.localPosition   = new Vector3(x, y, 0f);
                    
                    texts[i].text = _options[i];
                    texts[i].gameObject.SetActive(true);
                }
                else
                {
                    texts[i].gameObject.SetActive(false);
                }
                
                var text = texts[i];
            }
            


        }

        private void OnValidate()
        {
            Reload();
        }
    }
}
