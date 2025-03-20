using Common.Core.Boundary;
using Common.Infrastructure.Persistence;
using CSharpFunctionalExtensions;

namespace Presentation.Core.DataModels
{
    public class MonsterDetail : Entity<Guid>, IDataModel
    {
        public MonsterDetail(
            Guid id,
            string monsterName,
            int monsterLevel,
            SoulsAttributeSet attributeSet) : base(id)
        {
            MonsterName = monsterName;
            MonsterLevel = monsterLevel;
            AttributeSet = attributeSet;
        }

        public string MonsterName { get; set; } = null!;

        public int MonsterLevel { get; set; }

        public SoulsAttributeSet AttributeSet { get; set; } = null!;
    }
}
