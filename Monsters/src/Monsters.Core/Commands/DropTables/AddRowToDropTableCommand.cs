using Monsters.Core.Boundary.ValueObjects;

namespace Monsters.Core.Commands.DropTables
{
    public class AddRowToDropTableCommand
    {
        public AddRowToDropTableCommand(
            Guid tableId,
            Guid rowId,
            DropTableEntry entry)
        {
            TableId = tableId;
            RowId = rowId;
            Entry = entry;
        }

        public Guid TableId { get; private set; }
        public Guid RowId { get; private set; }
        public DropTableEntry Entry { get; private set; }
    }
}
