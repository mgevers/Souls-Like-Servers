using Ardalis.Result;

namespace Monsters.Core.Boundary.Events.DropTables
{
    public class FailedToRemoveRowFromDropTableEvent : FailureEvent
    {
        public FailedToRemoveRowFromDropTableEvent(
            Guid tableId,
            Guid rowId,
            ResultStatus status,
            IReadOnlyCollection<string> errors): base(status, errors)
        {
            TableId = tableId;
            RowId = rowId;
        }

        public Guid TableId { get; private set; }
        public Guid RowId { get; private set; }
    }
}
