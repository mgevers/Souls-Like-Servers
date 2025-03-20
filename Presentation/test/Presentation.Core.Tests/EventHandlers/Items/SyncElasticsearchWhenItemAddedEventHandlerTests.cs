using Common.Infrastructure.Persistence;
using Common.Testing.FluentTesting;
using Common.Testing.Persistence;
using Monsters.Core.Boundary.Events.Items;
using Monsters.Core.Boundary.ValueObjects;
using Moq.AutoMock;
using Presentation.Core.DataModels;
using Presentation.Core.EventHandlers.Items;
using Presentation.Testing;

namespace Presentation.Core.Tests.EventHandlers.Items
{
    public class SyncElasticsearchWhenItemAddedEventHandlerTests
    {
        [Fact]
        public async Task CanAddItem()
        {
            var item = DataModelDetails.CreateItem();

            await Arrange()
                .Handle(new ItemAddedEvent(item.Id, new ItemName(item.ItemName), item.AttributeSet))
                .AssertDatabase(new DatabaseState(item));
        }

        [Fact]
        public async Task AddItem_WhenCannotAddToDatabase_ThrowsException()
        {
            var item = DataModelDetails.CreateItem();
            var dbState = DatabaseState.Empty;

            await Arrange(dbState, isReadOnlyDatabase: true)
                .Handle(new ItemAddedEvent(item.Id, new ItemName(item.ItemName), item.AttributeSet))
                .AssertDatabase(dbState)
                .AssertExceptionThrown();
        }

        private static HandlerTestSetup<SyncElasticsearchWhenItemAddedEventHandler> Arrange(
                DatabaseState? databaseState = null,
                bool isReadOnlyDatabase = false)
        {
            return new HandlerTestSetup<SyncElasticsearchWhenItemAddedEventHandler>(
                databaseState ?? DatabaseState.Empty,
                isReadOnlyDatabase,
                ConfigureMocker);
        }

        private static void ConfigureMocker(AutoMocker mocker)
        {
            mocker.Use<IRepository<ItemDetail>>(new FakeRepository<ItemDetail>());
        }
    }
}
