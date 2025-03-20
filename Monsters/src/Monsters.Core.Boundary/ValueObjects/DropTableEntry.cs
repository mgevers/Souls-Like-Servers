using CSharpFunctionalExtensions;

namespace Monsters.Core.Boundary.ValueObjects
{
    public class DropTableEntry : ValueObject
    {
        public DropTableEntry(
            Guid itemId,
            DropRateDenominator dropRateDenominator)
        {
            ItemId = itemId;
            DropRateDenominator = dropRateDenominator;
        }

        private DropTableEntry() { }

        public Guid ItemId { get; private set; }
        public DropRateDenominator DropRateDenominator { get; private set; } = null!;

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            return [
                ItemId,
                DropRateDenominator,
            ];
        }
    }
}
