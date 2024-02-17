using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ToolTipSystem
{
    [ExecuteInEditMode]
    public class ControlLayoutElement : SerializedMonoBehaviour
    {
        public LayoutElement _layoutElement;
        public Dictionary<TextMeshProUGUI, int> _texts = new();

        private void LateUpdate()
        {
            if (_layoutElement is null) return;
            if (_texts is null) return;

            _layoutElement.enabled = NeedsToEnable();
        }

        [Button("Update Fields")]
        private void UpdateFields()
        {
            _layoutElement = GetComponent<LayoutElement>();
            _texts = GetComponentsInChildren<TextMeshProUGUI>().ToDictionary(tmp => tmp, _ => 0);
        }

        private bool NeedsToEnable() => 
            _texts.Any(text => text.Key.text.Length > text.Value);
    }
}
