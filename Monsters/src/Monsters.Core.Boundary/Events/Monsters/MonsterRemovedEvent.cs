namespace Monsters.Core.Boundary.Events.Monsters
{
    public class MonsterRemovedEvent
    {
        public MonsterRemovedEvent(Guid monsterId)
        {
            MonsterId = monsterId;
        }

        public Guid MonsterId { get; private set; }
    }
}
