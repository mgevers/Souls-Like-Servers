using Common.Infrastructure.Persistence;
using CSharpFunctionalExtensions;

namespace Presentation.Core.DataModels
{
    public class DropTableDetail : Entity<Guid>, IDataModel
    {
        public DropTableDetail(
            Guid id,
            MonsterDetail monster,
            int rollCount,
            IReadOnlyList<DropRateDetail> rows) : base(id)
        {
            Monster = monster;
            RollCount = rollCount;
            Rows = rows;
        }

        public MonsterDetail Monster { get; set; } = null!;

        public int RollCount { get; set; }

        public IReadOnlyList<DropRateDetail> Rows { get; set; } = [];
    }
}
