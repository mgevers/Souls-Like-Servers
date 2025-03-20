using Monsters.Core.Boundary.ValueObjects;

namespace Monster.Core.Boundary.Tests.ValueObjects
{
    public class RollCountTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void CanCreateRollCount(int count)
        {
            var rollCount = RollCount.Create(count);

            Assert.True(rollCount.IsSuccess);
            Assert.Equal(count, rollCount.Value);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-10)]
        [InlineData(-100)]
        public void CreateRollCount_WhenInvalid_ReturnFailureResult(int count)
        {
            var rollCount = RollCount.Create(count);

            Assert.True(rollCount.IsFailure);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-10)]
        [InlineData(-100)]
        public void ConstructRollCount_WhenInvalid_ThrowsException(int count)
        {
            Assert.ThrowsAny<Exception>(() =>
            {
                var rollCount = new RollCount(count);
            });
        }
    }
}
