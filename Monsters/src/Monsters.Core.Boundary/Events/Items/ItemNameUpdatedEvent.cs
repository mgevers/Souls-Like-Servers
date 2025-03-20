using Monsters.Core.Boundary.ValueObjects;

namespace Monsters.Core.Boundary.Events.Items
{
    public class ItemNameUpdatedEvent
    {
        public ItemNameUpdatedEvent(
            Guid itemId,
            ItemName itemName)
        {
            ItemId = itemId;
            ItemName = itemName;
        }

        public Guid ItemId { get; private set; }
        public ItemName ItemName { get; private set; }
    }
}
