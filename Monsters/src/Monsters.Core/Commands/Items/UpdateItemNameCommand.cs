using Monsters.Core.Boundary.ValueObjects;

namespace Monsters.Core.Commands.Items
{
    public class UpdateItemNameCommand
    {
        public UpdateItemNameCommand(
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
