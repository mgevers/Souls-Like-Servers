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
    public class AddItemCommandHandlerTests
    {
        [Fact]
        public async Task CanAddItem()
        {
            var item = Entities.CreateItem();

            await Arrange()
                .Handle(new AddItemCommand(item.Id, item.Name, item.AttributeSet))
                .AssertDatabase(new DatabaseState(item))
                .AssertPublishedEvent(new ItemAddedEvent(item.Id, item.Name, item.AttributeSet));
        }

        [Fact]
        public async Task AddItem_WhenCannotAddToDatabase_PublishesFailure()
        {
            var item = Entities.CreateItem();

            await Arrange(isReadOnlyDatabase: true)
                .Handle(new AddItemCommand(item.Id, item.Name, item.AttributeSet))
                .AssertDatabase(DatabaseState.Empty)
                .AssertPublishedEvent(new FailedToAddItemEvent(item.Id, ResultStatus.CriticalError, ["cannot write to readonly database"]));
        }

        [Fact]
        public async Task AddItem_WhenItemAreadyExists_PublishesFailure()
        {
            var item = Entities.CreateItem();
            var dbState = new DatabaseState(item);

            await Arrange(dbState)
                .Handle(new AddItemCommand(item.Id, item.Name, item.AttributeSet))
                .AssertDatabase(dbState)
                .AssertPublishedEvent(new FailedToAddItemEvent(item.Id, ResultStatus.Conflict, [$"conflict - entity with id {item.Id} already exists"]));
        }

        private static HandlerTestSetup<AddItemCommandHandler> Arrange(
                DatabaseState? databaseState = null,
                bool isReadOnlyDatabase = false)
        {
            return new HandlerTestSetup<AddItemCommandHandler>(
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
