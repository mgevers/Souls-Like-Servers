using Ardalis.Result;
using Common.Infrastructure.Persistence;
using Common.Testing.FluentTesting;
using Common.Testing.Persistence;
using Monsters.Core.Boundary.Events.Items;
using Monsters.Core.CommandHandlers.Items;
using Monsters.Core.Commands.Items;
using Monsters.Core.Domain;
using Monsters.Testing;
using Moq.AutoMock;

namespace Monsters.Core.Tests.CommandHandlers.Items
{
    public class RemoveItemCommandHandlerTests
    {
        [Fact]
        public async Task CanRemoveItem()
        {
            var item = Entities.CreateItem();

            await Arrange(new DatabaseState(item))
                .Handle(new RemoveItemCommand(item.Id))
                .AssertDatabase(DatabaseState.Empty)
                .AssertPublishedEvent(new ItemRemovedEvent(item.Id));
        }

        [Fact]
        public async Task RemoveItem_WhenCannotRemoveFromDatabase_PublishesFailure()
        {
            var item = Entities.CreateItem();
            var dbState = new DatabaseState(item);

            await Arrange(dbState, isReadOnlyDatabase: true)
                .Handle(new RemoveItemCommand(item.Id))
                .AssertDatabase(dbState)
                .AssertPublishedEvent(new FailedToRemoveItemEvent(item.Id, ResultStatus.CriticalError, ["cannot write to readonly database"]));
        }

        [Fact]
        public async Task RemoveItem_WhenDoesNotExist_PublishesFailure()
        {
            var item = Entities.CreateItem();
            var dbState = DatabaseState.Empty;

            await Arrange(dbState)
                .Handle(new RemoveItemCommand(item.Id))
                .AssertDatabase(dbState)
                .AssertPublishedEvent(new FailedToRemoveItemEvent(item.Id, ResultStatus.NotFound, [$"no saved entity with id: '{item.Id}'"]));
        }

        private static HandlerTestSetup<RemoveItemCommandHandler> Arrange(
                DatabaseState? databaseState = null,
                bool isReadOnlyDatabase = false)
        {
            return new HandlerTestSetup<RemoveItemCommandHandler>(
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
