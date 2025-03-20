using Monsters.Core.Boundary.ValueObjects;

namespace Monster.Core.Boundary.Tests.ValueObjects
{
    public class MonsterNameTests
    {
        [Theory]
        [InlineData("Skeleton")]
        [InlineData("Goblin")]
        [InlineData("Ice Dragon")]
        public void CanCreateMonsterName(string name)
        {
            var monsterName = MonsterName.Create(name);

            Assert.True(monsterName.IsSuccess);
            Assert.Equal(name, monsterName.Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void CreateMonsterName_WhenInvalid_ReturnFailureResult(string? name)
        {
            var monsterName = MonsterName.Create(name!);

            Assert.True(monsterName.IsFailure);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ConstructMonsterName_WhenInvalid_ThrowsException(string? name)
        {
            Assert.ThrowsAny<Exception>(() =>
            {
                var monsterName = new MonsterName(name!);
            });
        }
    }
}
