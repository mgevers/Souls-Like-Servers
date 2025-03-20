namespace Monsters.Core.Boundary.Events.DropTables
{
    public class RowRemovedFromDropTableEvent
    {
        public RowRemovedFromDropTableEvent(
            Guid tableId,
            Guid rowId)
        {
            TableId = tableId;
            RowId = rowId;
        }

        public Guid TableId { get; private set; }
        public Guid RowId { get; private set; }
    }
}
