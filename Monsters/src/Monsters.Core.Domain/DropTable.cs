using Ardalis.Result;
using Common.Infrastructure.Persistence;
using Monsters.Core.Boundary.ValueObjects;
using C = CSharpFunctionalExtensions;

namespace Monsters.Core.Domain
{
    public class DropTable : C.Entity<Guid>, IDataModel
    {
        public DropTable(
            Guid id,
            Monster monster,
            RollCount rollCount,
            IReadOnlyCollection<DropTableRow> rows) : base(id)
        {
            if (DropRatesGreaterThan100Percent(rows))
            {
                throw new ArgumentOutOfRangeException(nameof(rows), $"");
            }

            Monster = monster;
            RollCount = rollCount;
            Rows = [.. rows];
        }

        private DropTable() { }

        public Monster Monster { get; private set; } = null!;
        public RollCount RollCount { get; set; } = null!;
        public IReadOnlyList<DropTableRow> Rows { get; private set; } = null!;

        public Result<DropTable> AddRow(DropTableRow row)
        {
            var newRows = Rows.Concat([row]).ToList();

            if (DropRatesGreaterThan100Percent(newRows))
            {
                return Result<DropTable>.Invalid(new ValidationError("cannot add row, would make total drop rate greater than 100%"));
            }

            Rows = newRows;
            return Result<DropTable>.Success(this);
        }

        public void RemoveRow(Guid rowId)
        {
            if (!Rows.Any(row => row.Id == rowId))
            {
                throw new InvalidOperationException($"no row with id: {rowId} to remove");
            }

            var newRows = Rows
                .Where(row => row.Id != rowId)
                .ToList();

            Rows = newRows;
        }

        private static bool DropRatesGreaterThan100Percent(IReadOnlyCollection<DropTableRow> rows)
        {
            var totalDropRate = rows
                .Select(row => row.DropRate)
                .Sum();

            return totalDropRate > 1;
        }
    }
}
