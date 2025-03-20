using Monsters.Core.Boundary.ValueObjects;

namespace Monsters.Core.Boundary.Events.DropTables
{
    public class RowAddedToDropTableEvent
    {
        public RowAddedToDropTableEvent(
            Guid tableId,
            KeyValuePair<Guid, DropTableEntry> entry)
        {
            TableId = tableId;
            Entry = entry;
        }

        public Guid TableId { get; private set; }
        public KeyValuePair<Guid, DropTableEntry> Entry { get; }
    }
}
