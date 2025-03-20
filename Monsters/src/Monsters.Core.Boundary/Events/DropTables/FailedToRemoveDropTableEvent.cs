using Ardalis.Result;

namespace Monsters.Core.Boundary.Events.DropTables
{
    public class FailedToRemoveDropTableEvent : FailureEvent
    {
        public FailedToRemoveDropTableEvent(
            Guid tableId,
            ResultStatus status,
            IReadOnlyCollection<string> errors): base(status, errors)
        {
            TableId = tableId;
        }

        public Guid TableId { get; private set; }
    }
}
