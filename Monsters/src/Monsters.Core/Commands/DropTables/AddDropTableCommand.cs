using Monsters.Core.Boundary.ValueObjects;

namespace Monsters.Core.Commands.DropTables
{
    public class AddDropTableCommand
    {
        public AddDropTableCommand(
            Guid tableId,
            Guid monsterId,
            RollCount rollCount,
            IReadOnlyList<DropTableEntry> entries)
        {
            TableId = tableId;
            MonsterId = monsterId;
            RollCount = rollCount;
            Entries = [.. entries];
        }

        public Guid TableId { get; private set; }
        public Guid MonsterId { get; private set; }
        public RollCount RollCount { get; private set; }
        public IReadOnlyList<DropTableEntry> Entries { get; private set; }
    }
}
