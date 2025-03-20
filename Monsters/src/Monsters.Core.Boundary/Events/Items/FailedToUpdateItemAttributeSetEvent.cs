using Ardalis.Result;
using Common.Core.Boundary;

namespace Monsters.Core.Boundary.Events.Items
{
    public class FailedToUpdateItemAttributeSetEvent : FailureEvent
    {
        public FailedToUpdateItemAttributeSetEvent(
            Guid itemId,
            SoulsAttributeSet attributeSet,
            ResultStatus status,
            IReadOnlyCollection<string> errors) : base(status, errors)
        {
            ItemId = itemId;
            AttributeSet = attributeSet;
        }

        public Guid ItemId { get; private set; }
        public SoulsAttributeSet AttributeSet { get; private set; }
    }
}
