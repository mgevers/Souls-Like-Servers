using Monsters.Core.Boundary.ValueObjects;

namespace Monster.Core.Boundary.Tests.ValueObjects
{
    public class DropTableEntryTests
    {
        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(8)]
        public void CanCreateDropTableEntry(int denominator)
        {
            var id = Guid.NewGuid();
            var dropRate = new DropRateDenominator(denominator);
            var entry = new DropTableEntry(id, dropRate);

            Assert.Equal(id, entry.ItemId);
            Assert.Equal(dropRate, entry.DropRateDenominator);
        }
    }
}
