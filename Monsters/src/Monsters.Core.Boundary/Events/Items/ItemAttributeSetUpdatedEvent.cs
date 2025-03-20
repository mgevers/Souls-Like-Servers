using Common.Core.Boundary;

namespace Monsters.Core.Boundary.Events.Items
{
    public class ItemAttributeSetUpdatedEvent
    {
        public ItemAttributeSetUpdatedEvent(
            Guid itemId,
            SoulsAttributeSet attributeSet)
        {
            ItemId = itemId;
            AttributeSet = attributeSet;
        }

        public Guid ItemId { get; private set; }
        public SoulsAttributeSet AttributeSet { get; private set; }
    }
}
