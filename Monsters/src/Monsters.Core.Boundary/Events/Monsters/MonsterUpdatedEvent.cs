using Common.Core.Boundary;
using Monsters.Core.Boundary.ValueObjects;

namespace Monsters.Core.Boundary.Events.Monsters
{
    public class MonsterUpdatedEvent
    {
        public MonsterUpdatedEvent(
            Guid monsterId,
            MonsterName monsterName,
            MonsterLevel monsterLevel,
            SoulsAttributeSet attributeSet)
        {
            MonsterId = monsterId;
            MonsterName = monsterName;
            MonsterLevel = monsterLevel;
            AttributeSet = attributeSet;
        }

        public Guid MonsterId { get; private set; }
        public MonsterName MonsterName { get; private set; }
        public MonsterLevel MonsterLevel { get; private set; }
        public SoulsAttributeSet AttributeSet { get; private set; }
    }
}
