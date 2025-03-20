using Common.Infrastructure.Persistence;
using Common.Testing.FluentTesting;
using Common.Testing.Persistence;
using Monsters.Core.Boundary.Events.Items;
using Moq.AutoMock;
using Presentation.Core.DataModels;
using Presentation.Core.EventHandlers.Items;
using Presentation.Testing;

namespace Presentation.Core.Tests.EventHandlers.Items
{
    public class SyncElasticsearchWhenItemRemovedEventHandlerTests
    {
        [Fact]
        public async Task CanRemoveItem()
        {
            var item = DataModelDetails.CreateItem();

            await Arrange(new DatabaseState(item))
                .Handle(new ItemRemovedEvent(item.Id))
                .AssertDatabase(DatabaseState.Empty);
        }

        [Fact]
        public async Task RemoveItem_WhenCannotRemoveFromDatabase_ThrowsException()
        {
            var item = DataModelDetails.CreateItem();
            var dbState = new DatabaseState(item);

            await Arrange(dbState, isReadOnlyDatabase: true)
                .Handle(new ItemRemovedEvent(item.Id))
                .AssertDatabase(dbState)
                .AssertExceptionThrown();
        }

        private static HandlerTestSetup<SyncElasticsearchWhenItemRemovedEventHandler> Arrange(
                DatabaseState? databaseState = null,
                bool isReadOnlyDatabase = false)
        {
            return new HandlerTestSetup<SyncElasticsearchWhenItemRemovedEventHandler>(
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
