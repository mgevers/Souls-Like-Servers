using Common.Core.Boundary;
using CSharpFunctionalExtensions;
using Monsters.Core.Boundary.ValueObjects;

namespace Monsters.Core.Commands.Monsters
{
    public class AddMonsterCommand : Command
    {
        public AddMonsterCommand(
            Guid monsterId,
            MonsterName monsterName,
            MonsterLevel monsterLevel,
            SoulsAttributeSet attributeSet,
            string? connectionId = null) : base(connectionId)
        {
            MonsterId = monsterId;
            MonsterName = monsterName;
            MonsterLevel = monsterLevel;
            AttributeSet = attributeSet;
        }

        public static Result<AddMonsterCommand> Create(
            Guid monsterId,
            string monsterName,
            int monsterLevel,
            SoulsAttributeSet attributeSet,
            string? connectionId = null)
        {
            var name = MonsterName.Create(monsterName);
            var level = MonsterLevel.Create(monsterLevel);

            if (name.IsFailure)
            {
                return Result.Failure<AddMonsterCommand>(name.Error);
            }
            if (level.IsFailure)
            {
                return Result.Failure<AddMonsterCommand>(level.Error);
            }

            return new AddMonsterCommand(
                monsterId,
                name.Value,
                level.Value,
                attributeSet,
                connectionId);
        }

        public Guid MonsterId { get; private set; }
        public MonsterName MonsterName { get; private set; }
        public MonsterLevel MonsterLevel { get; private set; }
        public SoulsAttributeSet AttributeSet { get; private set; }
    }
}
