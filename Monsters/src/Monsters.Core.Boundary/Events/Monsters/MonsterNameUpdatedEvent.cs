using Monsters.Core.Boundary.ValueObjects;

namespace Monsters.Core.Boundary.Events.Monsters
{
    public class MonsterNameUpdatedEvent
    {
        public MonsterNameUpdatedEvent(
            Guid monsterId,
            MonsterName monsterName)
        {
            MonsterId = monsterId;
            MonsterName = monsterName;
        }

        public Guid MonsterId { get; private set; }
        public MonsterName MonsterName { get; private set; }
    }
}
