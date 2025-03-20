using Monsters.Core.Boundary.ValueObjects;

namespace Monsters.Core.Commands.DropTables
{
    public class UpdateDropTableRollCountCommand
    {
        public UpdateDropTableRollCountCommand(
            Guid tableId,
            RollCount rollCount)
        {
            TableId = tableId;
            RollCount = rollCount;
        }

        public Guid TableId { get; private set; }
        public RollCount RollCount { get; private set; }
    }
}
