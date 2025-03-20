using Ardalis.Result;

namespace Monsters.Core.Boundary.Events.Items
{
    public class FailedToRemoveItemEvent : FailureEvent
    {
        public FailedToRemoveItemEvent(
            Guid itemId,
            ResultStatus status,
            IReadOnlyCollection<string> errors) : base(status, errors)
        {
            ItemId = itemId;
        }

        public Guid ItemId { get; private set; }
    }
}
