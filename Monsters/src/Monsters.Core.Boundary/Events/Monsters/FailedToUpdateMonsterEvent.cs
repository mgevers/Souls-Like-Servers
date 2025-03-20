using Ardalis.Result;
using Common.Core.Boundary;
using Monsters.Core.Boundary.ValueObjects;

namespace Monsters.Core.Boundary.Events.Monsters
{
    public class FailedToUpdateMonsterEvent : FailureEvent
    {
        public FailedToUpdateMonsterEvent(
            Guid monsterId,
            MonsterName monsterName,
            MonsterLevel monsterLevel,
            SoulsAttributeSet attributeSet,
            ResultStatus status,
            IReadOnlyCollection<string> errors)
            : base(status, errors)
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
