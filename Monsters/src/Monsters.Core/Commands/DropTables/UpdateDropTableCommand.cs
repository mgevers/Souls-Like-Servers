using Monsters.Core.Boundary.ValueObjects;

namespace Monsters.Core.Commands.DropTables
{
    public class UpdateDropTableCommand
    {
        public UpdateDropTableCommand(
            Guid tableId,
            RollCount rollCount,
            IReadOnlyList<DropTableEntry> entries)
        {
            TableId = tableId;
            RollCount = rollCount;
            Entries = [.. entries];
        }

        public Guid TableId { get; }
        public RollCount RollCount { get; }
        public IReadOnlyList<DropTableEntry> Entries { get; }
    }
}
