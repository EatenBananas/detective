using Equipment;
using UnityEngine;

namespace Interactions
{
    public class ItemInteraction : MonoBehaviour
    {
        [SerializeField] private Item _requiredItem;
        [SerializeField] private Interaction _interaction;
        [SerializeField] private bool _removeItem = true;

        public void Use(Item item)
        {
            if (item == _requiredItem)
            {
                if (_removeItem)
                {
                    EquipmentManager.Instance.RemoveActiveItem();
                }
                InteractionManager.Instance.StartInteraction(_interaction);
            }
        }
    }
}
