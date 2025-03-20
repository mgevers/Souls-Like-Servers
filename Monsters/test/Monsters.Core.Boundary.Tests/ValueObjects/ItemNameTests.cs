using Monsters.Core.Boundary.ValueObjects;

namespace Monster.Core.Boundary.Tests.ValueObjects
{
    public class ItemNameTests
    {
        [Theory]
        [InlineData("Skeleton")]
        [InlineData("Goblin")]
        [InlineData("Ice Dragon")]
        public void CanCreateItemName(string name)
        {
            var itemName = ItemName.Create(name);

            Assert.True(itemName.IsSuccess);
            Assert.Equal(name, itemName.Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void CreateItemName_WhenInvalid_ReturnFailureResult(string? name)
        {
            var itemName = ItemName.Create(name!);

            Assert.True(itemName.IsFailure);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ConstructItemName_WhenInvalid_ThrowsException(string? name)
        {
            Assert.ThrowsAny<Exception>(() =>
            {
                var monsterName = new ItemName(name!);
            });
        }
    }
}
