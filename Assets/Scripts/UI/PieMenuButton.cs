using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class PieMenuButton : Image
    {
        // this code overrides raycast logic, so that the area hidden by mask is not selectable
        public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera) {
            bool result = base.IsRaycastLocationValid(screenPoint, eventCamera);
            if (!result) {
                return false;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out var localPoint);
            float clickAngle = Vector2.SignedAngle(localPoint, new Vector2(0, -1));
            return (clickAngle >= 0) && (clickAngle < (360f * fillAmount));
        }
        private void Start()
        {
            alphaHitTestMinimumThreshold = 0.5f;
        }

        public void Reload(float fill, float angle, string text, float radius, int buttons)
        {
            fillAmount = fill;
            rectTransform.eulerAngles = new Vector3(0f, 0f, -angle);

            var _textField = GetComponentInChildren<TextMeshProUGUI>();
            if (_textField == null)
            {
                Debug.LogError("PieMenuButton: text field not found!");
                return;
            }

            _textField.text = text;
            _textField.rectTransform.eulerAngles = Vector3.zero;

            // Konwertuj kąt na radiany
            float angleInRadians = Mathf.PI / buttons * (buttons - 1); //- Mathf.PI / buttons;

            // Oblicz współrzędne x i y na obwodzie koła
            float x = radius * Mathf.Cos(angleInRadians);
            float y = radius * Mathf.Sin(angleInRadians);

            // Ustaw pozycję obiektu na podstawie obliczonych współrzędnych
            _textField.rectTransform.localPosition = new Vector3(x, y, 0f);
            
        }
    }
}
