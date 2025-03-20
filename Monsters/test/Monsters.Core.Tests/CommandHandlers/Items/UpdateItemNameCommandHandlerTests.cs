using Ardalis.Result;
using Common.Infrastructure.Persistence;
using Common.Testing.FluentTesting;
using Common.Testing.Persistence;
using Monsters.Core.Boundary.Events.Items;
using Monsters.Core.Boundary.ValueObjects;
using Monsters.Core.CommandHandlers.Items;
using Monsters.Core.Commands.Items;
using Monsters.Core.Domain;
using Monsters.Testing;
using Moq.AutoMock;
using Newtonsoft.Json;

namespace Monsters.Core.Tests.CommandHandlers.Items
{
    public class UpdateItemNameCommandHandlerTests
    {
        [Fact]
        public async Task CanUpdateItemName()
        {
            var item = Entities.CreateItem();
            var newName = new ItemName("Sword of Morne");

            var endItem = JsonConvert.DeserializeObject<Item>(JsonConvert.SerializeObject(item))!;
            endItem.Name = newName;

            await Arrange(new DatabaseState(item))
                .Handle(new UpdateItemNameCommand(item.Id, newName))
                .AssertDatabase(new DatabaseState(endItem))
                .AssertPublishedEvent(new ItemNameUpdatedEvent(item.Id, newName));
        }

        [Fact]
        public async Task UpdateItemName_WhenCannotRemoveFromDatabase_PublishesFailure()
        {
            var item = Entities.CreateItem();
            var dbState = new DatabaseState(item);
            var newName = new ItemName("Sword of Morne");

            await Arrange(dbState, isReadOnlyDatabase: true)
                .Handle(new UpdateItemNameCommand(item.Id, newName))
                .AssertDatabase(dbState)
                .AssertPublishedEvent(new FailedToUpdateItemNameEvent(item.Id, newName, ResultStatus.CriticalError, ["cannot write to readonly database"]));
        }

        [Fact]
        public async Task UpdateItemName_WhenDoesNotExist_PublishesFailure()
        {
            var item = Entities.CreateItem();
            var dbState = DatabaseState.Empty;
            var newName = new ItemName("Sword of Morne");

            await Arrange(dbState)
                .Handle(new UpdateItemNameCommand(item.Id, newName))
                .AssertDatabase(dbState)
                .AssertPublishedEvent(new FailedToUpdateItemNameEvent(item.Id, newName, ResultStatus.NotFound, [$"no saved entity with id: '{item.Id}'"]));
        }

        private static HandlerTestSetup<UpdateItemNameCommandHandler> Arrange(
                DatabaseState? databaseState = null,
                bool isReadOnlyDatabase = false)
        {
            return new HandlerTestSetup<UpdateItemNameCommandHandler>(
                databaseState ?? DatabaseState.Empty,
                isReadOnlyDatabase,
                ConfigureMocker);
        }

        private static void ConfigureMocker(AutoMocker mocker)
        {
            mocker.Use<IRepository<Item>>(new FakeRepository<Item>());
        }
    }
}
