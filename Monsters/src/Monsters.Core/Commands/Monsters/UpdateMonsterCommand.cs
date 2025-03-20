using Common.Core.Boundary;
using CSharpFunctionalExtensions;
using Monsters.Core.Boundary.ValueObjects;

namespace Monsters.Core.Commands.Monsters
{
    public class UpdateMonsterCommand
    {
        public UpdateMonsterCommand(
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

        public static Result<UpdateMonsterCommand> Create(
            Guid monsterId,
            string monsterName,
            int monsterLevel,
            SoulsAttributeSet attributeSet)
        {
            var name = MonsterName.Create(monsterName);
            var level = MonsterLevel.Create(monsterLevel);

            if (name.IsFailure)
            {
                return Result.Failure<UpdateMonsterCommand>(name.Error);
            }
            if (level.IsFailure)
            {
                return Result.Failure<UpdateMonsterCommand>(level.Error);
            }

            return new UpdateMonsterCommand(monsterId, name.Value, level.Value, attributeSet);
        }

        public Guid MonsterId { get; private set; }
        public MonsterName MonsterName { get; private set; }
        public MonsterLevel MonsterLevel { get; private set; }
        public SoulsAttributeSet AttributeSet { get; private set; }
    }
}
