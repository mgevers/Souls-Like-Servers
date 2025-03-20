using Ardalis.Result;
using Common.Core.Boundary;
using Monsters.Core.Boundary.ValueObjects;

namespace Monsters.Core.Boundary.Events.Items
{
    public class FailedToUpdateItemEvent : FailureEvent
    {
        public FailedToUpdateItemEvent(
            Guid itemId,
            ItemName itemName,
            SoulsAttributeSet attributeSet,
            ResultStatus status,
            IReadOnlyCollection<string> errors) : base(status, errors)
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
