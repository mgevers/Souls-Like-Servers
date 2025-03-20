using Ardalis.Result;
using Common.Core.Boundary;

namespace Monsters.Core.Boundary.Events.Monsters
{
    public class FailedToUpdateMonsterAttributeSetEvent : FailureEvent
    {
        public FailedToUpdateMonsterAttributeSetEvent(
            Guid monsterId,
            SoulsAttributeSet attributeSet,
            ResultStatus status,
            IReadOnlyCollection<string> errors) : base(status, errors)
        {
            MonsterId = monsterId;
            AttributeSet = attributeSet;
        }

        public Guid MonsterId { get; private set; }
        public SoulsAttributeSet AttributeSet { get; private set; }
    }
}
