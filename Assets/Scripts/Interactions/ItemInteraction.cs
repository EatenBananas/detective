using Equipment;
using Interactions.Elements;
using UnityEngine;

namespace Interactions
{
    public class ItemInteraction : MonoBehaviour
    {
        [SerializeField] private Item _requiredItem;
        [SerializeField] private StartInteraction _interaction;
        [SerializeField] private bool _removeItem = true;

        public void Use(Item item)
        {
            if (item == _requiredItem)
            {
                InteractionManager.Instance.StartInteraction(_interaction);
            }
        }
    }
}
