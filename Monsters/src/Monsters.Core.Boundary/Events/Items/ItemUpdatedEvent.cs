using Common.Core.Boundary;
using Monsters.Core.Boundary.ValueObjects;

namespace Monsters.Core.Boundary.Events.Items
{
    public class ItemUpdatedEvent
    {
        public ItemUpdatedEvent(
            Guid itemId,
            ItemName itemName,
            SoulsAttributeSet attributeSet)
        {
            ItemId = itemId;
            ItemName = itemName;
            AttributeSet = attributeSet;
        }

        public Guid ItemId { get; private set; }
        public ItemName ItemName { get; private set; }
        public SoulsAttributeSet AttributeSet { get; private set; }
    }
}
