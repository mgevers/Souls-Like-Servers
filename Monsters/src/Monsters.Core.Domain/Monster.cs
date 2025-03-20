using Common.Core.Boundary;
using Common.Infrastructure.Persistence;
using CSharpFunctionalExtensions;
using Monsters.Core.Boundary.ValueObjects;

namespace Monsters.Core.Domain
{
    public class Monster : Entity<Guid>, IDataModel
    {
        public Monster(
            Guid id,
            MonsterName name,
            MonsterLevel level,
            SoulsAttributeSet attributeSet) : base(id)
        {
            Name = name;
            Level = level;
            AttributeSet = attributeSet;
        }

        private Monster() { }

        public MonsterName Name { get; set; } = null!;
        public MonsterLevel Level { get; set; } = null!;
        public SoulsAttributeSet AttributeSet { get; set; } = null!;
    }
}
