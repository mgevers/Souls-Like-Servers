using Ardalis.Result;
using Monsters.Core.Boundary.ValueObjects;

namespace Monsters.Core.Boundary.Events.Monsters
{
    public class FailedToUpdateMonsterLevelEvent : FailureEvent
    {
        public FailedToUpdateMonsterLevelEvent(
            Guid monsterId,
            MonsterLevel monsterLevel,
            ResultStatus status,
            IReadOnlyCollection<string> errors) : base(status, errors)
        {
            MonsterId = monsterId;
            MonsterLevel = monsterLevel;
        }

        public Guid MonsterId { get; private set; }
        public MonsterLevel MonsterLevel { get; private set; }
    }
}
