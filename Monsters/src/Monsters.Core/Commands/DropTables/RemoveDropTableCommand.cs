namespace Monsters.Core.Commands.DropTables
{
    public class RemoveDropTableCommand
    {
        public RemoveDropTableCommand(Guid tableId)
        {
            TableId = tableId;
        }

        public Guid TableId { get; private set; }
    }
}
