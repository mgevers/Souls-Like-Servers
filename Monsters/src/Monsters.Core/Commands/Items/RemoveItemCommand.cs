namespace Monsters.Core.Commands.Items
{
    public class RemoveItemCommand
    {
        public RemoveItemCommand(Guid itemId)
        {
            ItemId = itemId;
        }

        public Guid ItemId { get; private set; }
    }
}
