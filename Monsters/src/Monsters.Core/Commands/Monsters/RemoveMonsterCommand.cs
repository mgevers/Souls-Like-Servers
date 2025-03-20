namespace Monsters.Core.Commands.Monsters
{
    public class RemoveMonsterCommand
    {
        public RemoveMonsterCommand(Guid monsterId)
        {
            MonsterId = monsterId;
        }

        public Guid MonsterId { get; private set; }
    }
}
