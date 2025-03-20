using Monsters.Core.Boundary.ValueObjects;

namespace Monster.Core.Boundary.Tests.ValueObjects
{
    public class MonsterLevelTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(50)]
        [InlineData(100)]
        public void CanCreateMonsterLevel(int level)
        {
            var monsterLevel = MonsterLevel.Create(level);

            Assert.True(monsterLevel.IsSuccess);
            Assert.Equal(level, monsterLevel.Value);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(101)]
        public void CreateMonsterLevel_WhenInvalid_ReturnFailureResult(int level)
        {
            var monsterLevel = MonsterLevel.Create(level);

            Assert.True(monsterLevel.IsFailure);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(101)]
        public void ConstructMonsterLevel_WhenInvalid_ThrowsException(int level)
        {
            Assert.ThrowsAny<Exception>(() =>
            {
                var monsterLevel = new MonsterLevel(level);
            });
        }
    }
}
