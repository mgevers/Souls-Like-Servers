using Ardalis.Result;
using Monsters.Core.Boundary.ValueObjects;

namespace Monsters.Core.Boundary.Events.DropTables
{
    public class FailedToUpdateDropTableRollCountEvent : FailureEvent
    {
        public FailedToUpdateDropTableRollCountEvent(
            Guid tableId,
            RollCount rollCount,
            ResultStatus status,
            IReadOnlyCollection<string> errors) : base(status, errors)
        {
            TableId = tableId;
            RollCount = rollCount;
        }

        public Guid TableId { get; private set; }
        public RollCount RollCount { get; private set; }
    }
}
