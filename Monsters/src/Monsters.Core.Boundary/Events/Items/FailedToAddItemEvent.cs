using Ardalis.Result;

namespace Monsters.Core.Boundary.Events.Items
{
    public class FailedToAddItemEvent : FailureEvent
    {
        public FailedToAddItemEvent(
            Guid itemId,
            ResultStatus status, 
            IReadOnlyCollection<string> errors) : base(status, errors)
        {
            ItemId = itemId;
        }

        public Guid ItemId { get; private set; }
    }
}
