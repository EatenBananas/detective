using Equipment;

namespace Interactions.Elements
{
    public class Equip : InteractionElement
    {
        private Item _item;
        public override void Execute()
        {
            EquipmentManager.Instance.Equip(_item);
            InteractionManager.Instance.CompleteElement();
        }

        public Equip(InteractionElementData data)
        {
            _item = data.Item;
        }
    }
}