using Monsters.Core.Boundary.ValueObjects;

namespace Monster.Core.Boundary.Tests.ValueObjects
{
    public class DropRateDenominatorTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(2048)]
        public void CanCreateDropRateDenominator(int rate)
        {
            var rollCount = DropRateDenominator.Create(rate);

            Assert.True(rollCount.IsSuccess);
            Assert.Equal(rate, rollCount.Value);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(3)]
        [InlineData(9)]
        public void CreateDropRateDenominator_WhenInvalid_ReturnFailureResult(int rate)
        {
            var rollCount = DropRateDenominator.Create(rate);

            Assert.True(rollCount.IsFailure);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(3)]
        [InlineData(9)]
        public void ConstructDropRateDenominator_WhenInvalid_ThrowsException(int rate)
        {
            Assert.ThrowsAny<Exception>(() =>
            {
                var rollCount = new DropRateDenominator(rate);
            });
        }
    }
}
