using CSharpFunctionalExtensions;
using Monsters.Core.Boundary.ValueObjects;

namespace Monsters.Core.Domain
{
    public class DropTableRow : Entity<Guid>
    {
        public DropTableRow(
            Guid id,
            Item item,
            DropRateDenominator dropRateDenominator) : base(id)
        {
            Item = item;
            DropRateDenominator = dropRateDenominator;
        }

        private DropTableRow() { }

        public Item Item { get; private set; } = null!;
        public DropRateDenominator DropRateDenominator { get; private set; } = null!;

        public double DropRate => 1 / (double)DropRateDenominator;
    }
}
