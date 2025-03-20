using Monsters.Core.Boundary.ValueObjects;

namespace Monsters.Core.Commands.Monsters
{
    public class UpdateMonsterLevelCommand
    {
        public UpdateMonsterLevelCommand(
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
