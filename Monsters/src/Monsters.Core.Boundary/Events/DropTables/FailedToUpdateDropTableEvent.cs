using Ardalis.Result;
using Monsters.Core.Boundary.ValueObjects;

namespace Monsters.Core.Boundary.Events.DropTables
{
    public class FailedToUpdateDropTableEvent : FailureEvent
    {
        public FailedToUpdateDropTableEvent(
            Guid tableId,
            RollCount rollCount,
            IReadOnlyList<KeyValuePair<Guid, DropTableEntry>> entries,
            ResultStatus status,
            IReadOnlyCollection<string> errors) : base(status, errors)
        {
            TableId = tableId;
            RollCount = rollCount;
            Entries = [.. entries];
        }

        public Guid TableId { get; private set; }
        public RollCount RollCount { get; private set; }
        public IReadOnlyList<KeyValuePair<Guid, DropTableEntry>> Entries { get; private set; }
    }
}
