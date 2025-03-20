namespace Monsters.Core.Commands
{
    public class Command
    {
        public Command(string? connectionId)
        {
            ConnectionId = connectionId;
        }

        public string? ConnectionId { get; private set; }
    }
}
