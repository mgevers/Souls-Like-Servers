using Monsters.Core.Boundary.ValueObjects;

namespace Monsters.Core.Commands.Monsters
{
    public class UpdateMonsterNameCommand
    {
        public UpdateMonsterNameCommand(
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
