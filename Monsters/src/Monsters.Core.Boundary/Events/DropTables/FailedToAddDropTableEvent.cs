using Ardalis.Result;

namespace Monsters.Core.Boundary.Events.DropTables
{
    public class FailedToAddDropTableEvent : FailureEvent
    {
        public FailedToAddDropTableEvent(
            Guid tableId,
            ResultStatus status,
            IReadOnlyCollection<string> errors) : base(status, errors)
        {
            TableId = tableId;
        }

        public Guid TableId { get; private set; }
    }
}
