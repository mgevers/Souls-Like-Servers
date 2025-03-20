using Ardalis.Result;
using Common.Infrastructure.Persistence;
using Common.Testing.FluentTesting;
using Common.Testing.Persistence;
using Monsters.Core.Boundary.Events.DropTables;
using Monsters.Core.CommandHandlers.DropTables;
using Monsters.Core.Commands.DropTables;
using Monsters.Core.Domain;
using Monsters.Testing;
using Moq.AutoMock;

namespace Monsters.Core.Tests.CommandHandlers.DropTables
{
    public class AddDropTableCommandHandlerTests
    {
        [Fact]
        public async Task CanAddDropTable()
        {
            var monster = Entities.CreateMonster();
            var dropTable = Entities.CreateEmptyDropTable(monster: monster);

            await Arrange(new DatabaseState(monster))
                .Handle(new AddDropTableCommand(dropTable.Id, dropTable.Monster.Id, dropTable.RollCount, []))
                .AssertDatabase(new DatabaseState(dropTable, monster))
                .AssertPublishedEvent(new DropTableAddedEvent(dropTable.Id, dropTable.Monster.Id, dropTable.RollCount, []));
        }

        [Fact]
        public async Task AddDropTable_WhenCannotAddToDatabase_PublishesFailure()
        {
            var monster = Entities.CreateMonster();
            var dropTable = Entities.CreateEmptyDropTable(monster: monster);
            var dbState = new DatabaseState(monster);

            await Arrange(dbState, isReadOnlyDatabase: true)
                .Handle(new AddDropTableCommand(dropTable.Id, dropTable.Monster.Id, dropTable.RollCount, []))
                .AssertDatabase(dbState)
                .AssertPublishedEvent(new FailedToAddDropTableEvent(dropTable.Id, ResultStatus.CriticalError, ["cannot write to readonly database"]));
        }

        [Fact]
        public async Task AddDropTable_WhenTableAreadyExists_PublishesFailure()
        {
            var monster = Entities.CreateMonster();
            var dropTable = Entities.CreateEmptyDropTable(monster: monster);
            var dbState = new DatabaseState(dropTable, monster);

            await Arrange(dbState)
                .Handle(new AddDropTableCommand(dropTable.Id, dropTable.Monster.Id, dropTable.RollCount, []))
                .AssertDatabase(dbState)
                .AssertPublishedEvent(new FailedToAddDropTableEvent(dropTable.Id, ResultStatus.Conflict, [$"conflict - entity with id {dropTable.Id} already exists"]));
        }

        private static HandlerTestSetup<AddDropTableCommandHandler> Arrange(
            DatabaseState? databaseState = null,
            bool isReadOnlyDatabase = false)
        {
            return new HandlerTestSetup<AddDropTableCommandHandler>(
                databaseState ?? DatabaseState.Empty,
                isReadOnlyDatabase,
                ConfigureMocker);
        }

        private static void ConfigureMocker(AutoMocker mocker)
        {
            mocker.Use<IRepository<DropTable>>(new FakeRepository<DropTable>());
            mocker.Use<IRepository<Item>>(new FakeRepository<Item>());
            mocker.Use<IRepository<Monster>>(new FakeRepository<Monster>());
        }
    }
}
