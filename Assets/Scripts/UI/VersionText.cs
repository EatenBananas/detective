using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class VersionText : MonoBehaviour
    {
        private void Start()
        {
            Reload();
        }

        [ContextMenu("Reload")]
        private void Reload()
        {
            var textMesh = GetComponent<TextMeshProUGUI>();
            textMesh.text = $"Version {Application.version}";
        }
    }
}
