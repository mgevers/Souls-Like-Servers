using Ardalis.Result;

namespace Monsters.Core.Boundary.Events.Monsters
{
    public class FailedToRemoveMonsterEvent : FailureEvent
    {
        public FailedToRemoveMonsterEvent(
            Guid monsterId,
            ResultStatus status,
            IReadOnlyCollection<string> errors) : base(status, errors)
        {
            MonsterId = monsterId;
        }

        public Guid MonsterId { get; private set; }
    }
}
