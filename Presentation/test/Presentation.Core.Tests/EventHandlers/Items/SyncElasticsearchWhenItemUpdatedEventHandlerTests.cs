using Common.Core.Boundary;
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
    public class SyncElasticsearchWhenItemUpdatedEventHandlerTests
    {
        [Fact]
        public async Task CanUpdateItemAttributeSet()
        {
            var item = DataModelDetails.CreateItem();
            var updatedItem = DataModelDetails.CreateItem(
                id: item.Id,
                attributeSet: new SoulsAttributeSet(physicalPower: 10));

            await Arrange(new DatabaseState(item))
                .Handle(new ItemAttributeSetUpdatedEvent(item.Id, updatedItem.AttributeSet))
                .AssertDatabase(new DatabaseState(updatedItem));
        }

        [Fact]
        public async Task UpdateItemAttributeSet_WhenCannotModifyDatabase_ThrowsException()
        {
            var item = DataModelDetails.CreateItem();
            var updatedItem = DataModelDetails.CreateItem(
                id: item.Id,
                attributeSet: new SoulsAttributeSet(physicalPower: 10));
            var dbState = new DatabaseState(item);

            await Arrange(dbState, isReadOnlyDatabase: true)
                .Handle(new ItemAttributeSetUpdatedEvent(item.Id, updatedItem.AttributeSet))
                .AssertDatabase(dbState)
                .AssertExceptionThrown();
        }

        [Fact]
        public async Task CanUpdateItemName()
        {
            var item = DataModelDetails.CreateItem();
            var updatedItem = DataModelDetails.CreateItem(
                id: item.Id,
                itemName: "Flail");

            await Arrange(new DatabaseState(item))
                .Handle(new ItemNameUpdatedEvent(item.Id, new ItemName(updatedItem.ItemName)))
                .AssertDatabase(new DatabaseState(updatedItem));
        }

        [Fact]
        public async Task UpdateItemName_WhenCannotModifyDatabase_ThrowsException()
        {
            var item = DataModelDetails.CreateItem();
            var updatedItem = DataModelDetails.CreateItem(
                id: item.Id,
                itemName: "Flail");
            var dbState = new DatabaseState(item);

            await Arrange(dbState, isReadOnlyDatabase: true)
                .Handle(new ItemNameUpdatedEvent(item.Id, new ItemName(updatedItem.ItemName)))
                .AssertDatabase(dbState)
                .AssertExceptionThrown();
        }

        private static HandlerTestSetup<SyncElasticsearchWhenItemUpdatedEventHandler> Arrange(
                DatabaseState? databaseState = null,
                bool isReadOnlyDatabase = false)
        {
            return new HandlerTestSetup<SyncElasticsearchWhenItemUpdatedEventHandler>(
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
