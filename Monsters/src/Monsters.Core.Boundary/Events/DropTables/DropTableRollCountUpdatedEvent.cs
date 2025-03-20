using Monsters.Core.Boundary.ValueObjects;

namespace Monsters.Core.Boundary.Events.DropTables
{
    public class DropTableRollCountUpdatedEvent
    {
        public DropTableRollCountUpdatedEvent(
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
