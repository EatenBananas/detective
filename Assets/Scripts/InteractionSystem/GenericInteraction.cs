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

        public void OnEnter() => OnMouseEnterEvent?.Invoke();

        public void OnStay() => OnMouseStayEvent?.Invoke();

        public void OnDown() => OnMouseDownEvent?.Invoke();

        public void OnExit() => OnMouseExitEvent?.Invoke();
    }
}