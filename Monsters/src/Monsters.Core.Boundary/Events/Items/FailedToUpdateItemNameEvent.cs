using Ardalis.Result;
using Monsters.Core.Boundary.ValueObjects;

namespace Monsters.Core.Boundary.Events.Items
{
    public class FailedToUpdateItemNameEvent : FailureEvent
    {
        public FailedToUpdateItemNameEvent(
            Guid itemId,
            ItemName itemName,
            ResultStatus status, 
            IReadOnlyCollection<string> errors) : base(status, errors)
        {
            ItemId = itemId;
            ItemName = itemName;
        }

        public Guid ItemId { get; private set; }
        public ItemName ItemName { get; private set; }
    }
}
