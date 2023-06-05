using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class PieMenu : MonoBehaviour
    {
        [SerializeField] private float _spacing = 0.05f;
        [SerializeField] private float _radius = 100f;
        [SerializeField] private List<string> _options = new List<string>();

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
                    buttons[i].Reload(fill, 360f / _options.Count * i, _options[i], _radius, _options.Count);
                    buttons[i].gameObject.SetActive(true);
                }
                else
                {
                    buttons[i].gameObject.SetActive(false);
                }
            }
            
        }

        private void OnValidate()
        {
            Reload();
        }
    }
}
