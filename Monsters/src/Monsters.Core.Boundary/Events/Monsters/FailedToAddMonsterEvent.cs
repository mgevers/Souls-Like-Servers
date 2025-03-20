using Ardalis.Result;

namespace Monsters.Core.Boundary.Events.Monsters
{
    public class FailedToAddMonsterEvent : FailureEvent
    {
        public FailedToAddMonsterEvent(
            Guid monsterId,
            ResultStatus status,
            IReadOnlyCollection<string> errors,
            string? connectionId = null) : base(status, errors, connectionId)
        {
            MonsterId = monsterId;
        }

        public Guid MonsterId { get; private set; }
    }
}
