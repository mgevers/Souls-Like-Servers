using Monsters.Core.Boundary.ValueObjects;

namespace Monsters.Core.Boundary.Events.Monsters
{
    public class MonsterLevelUpdatedEvent
    {
        public MonsterLevelUpdatedEvent(
            Guid monsterId,
            MonsterLevel monsterLevel)
        {
            MonsterId = monsterId;
            MonsterLevel = monsterLevel;
        }

        public Guid MonsterId { get; private set; }
        public MonsterLevel MonsterLevel { get; private set; }
    }
}
