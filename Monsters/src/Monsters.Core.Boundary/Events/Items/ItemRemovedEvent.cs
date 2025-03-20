namespace Monsters.Core.Boundary.Events.Items
{
    public class ItemRemovedEvent
    {
        public ItemRemovedEvent(Guid itemId)
        {
            ItemId = itemId;
        }

        public Guid ItemId { get; private set; }
    }
}
