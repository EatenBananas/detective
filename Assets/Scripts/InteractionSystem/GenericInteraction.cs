using UnityEngine;
using UnityEngine.Events;

namespace InteractionSystem
{
    public class GenericInteraction : MonoBehaviour, IInteraction
    {
        [SerializeField] private bool _canInteract = true;

        public UnityEvent OnMouseEnterEvent;
        public UnityEvent OnMouseStayEvent;
        public UnityEvent OnMouseDownEvent;
        public UnityEvent OnMouseExitEvent;

        public bool CanInteract() => _canInteract;

        public void OnEnter()
        {
            Debug.Log($"OnMouseEnter: {gameObject.name}", this);
            OnMouseEnterEvent?.Invoke();
        }

        public void OnStay()
        {
            Debug.Log($"OnMouseStay: {gameObject.name}", this);
            OnMouseStayEvent?.Invoke();
        }

        public void OnDown()
        {
            Debug.Log($"OnMouseDown: {gameObject.name}", this);
            OnMouseDownEvent?.Invoke();
        }

        public void OnExit()
        {
            Debug.Log($"OnMouseExit: {gameObject.name}", this);
            OnMouseExitEvent?.Invoke();
        }
    }
}