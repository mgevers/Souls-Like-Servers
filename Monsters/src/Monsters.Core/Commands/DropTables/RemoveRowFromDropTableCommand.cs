namespace Monsters.Core.Commands.DropTables
{
    public class RemoveRowFromDropTableCommand
    {
        public RemoveRowFromDropTableCommand(Guid tableId, Guid rowId)
        {
            TableId = tableId;
            RowId = rowId;
        }

        public Guid TableId { get; private set; }
        public Guid RowId { get; private set; }
    }
}
