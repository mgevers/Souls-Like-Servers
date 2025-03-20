namespace Monsters.Core.Boundary.Events.DropTables
{
    public class DropTableRemovedEvent
    {
        public DropTableRemovedEvent(Guid tableId)
        {
            TableId = tableId;
        }

        public Guid TableId { get; private set; }
    }
}
