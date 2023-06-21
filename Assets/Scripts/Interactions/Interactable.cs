using System;
using UnityEngine;

namespace Interactions
{
    [RequireComponent(typeof(SphereCollider))]
    public class Interactable : MonoBehaviour
    {
        private static readonly Color INTERACTABLE_GIZMO_COLOR = 
            new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 0.2f);

        [field: SerializeField] public string Text { get; private set; } = "";
        [field: SerializeField] public Interaction Interaction { get; private set; }

        private void OnDrawGizmosSelected()
        {
            var trigger = GetComponent<SphereCollider>();
            var scale = transform.lossyScale;

            Gizmos.color = INTERACTABLE_GIZMO_COLOR;
            Gizmos.DrawSphere(transform.position + trigger.center,
                trigger.radius * Math.Max(scale.x, Math.Max(scale.y, scale.z)));
        }

        // temp
        private void OnTriggerEnter(Collider other)
        {
            InteractionManager.Instance.Enter(this);
        }

        private void OnTriggerExit(Collider other)
        {
            InteractionManager.Instance.Exit();
        }
    }
}
