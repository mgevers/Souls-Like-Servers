using Ardalis.Result;
using Monsters.Core.Boundary.ValueObjects;

namespace Monsters.Core.Boundary.Events.Monsters
{
    public class FailedToUpdateMonsterNameEvent : FailureEvent
    {
        public FailedToUpdateMonsterNameEvent(
            Guid monsterId,
            MonsterName monsterName,
            ResultStatus status,
            IReadOnlyCollection<string> errors) : base(status, errors)
        {
            MonsterId = monsterId;
            MonsterName = monsterName;
        }

        public Guid MonsterId { get; private set; }
        public MonsterName MonsterName { get; private set; }
    }
}
