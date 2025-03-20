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
    public class RemoveDropTableCommandHandlerTests
    {
        [Fact]
        public async Task CanRemoveDropTable()
        {
            var dropTable = Entities.CreateEmptyDropTable();

            await Arrange(new DatabaseState(dropTable))
                .Handle(new RemoveDropTableCommand(dropTable.Id))
                .AssertDatabase(DatabaseState.Empty)
                .AssertPublishedEvent(new DropTableRemovedEvent(dropTable.Id));
        }

        [Fact]
        public async Task RemoveDropTable_WhenCannotRemoveFromDatabase_PublishesFailure()
        {
            var dropTable = Entities.CreateEmptyDropTable();
            var dbState = new DatabaseState(dropTable);

            await Arrange(dbState, isReadOnlyDatabase: true)
                .Handle(new RemoveDropTableCommand(dropTable.Id))
                .AssertDatabase(dbState)
                .AssertPublishedEvent(new FailedToRemoveDropTableEvent(dropTable.Id, ResultStatus.CriticalError, ["cannot write to readonly database"]));
        }

        [Fact]
        public async Task RemoveDropTable_WhenTableNotFound_PublishesFailure()
        {
            var dropTable = Entities.CreateEmptyDropTable();
            var dbState = DatabaseState.Empty;

            await Arrange(dbState)
                .Handle(new RemoveDropTableCommand(dropTable.Id))
                .AssertDatabase(dbState)
                .AssertPublishedEvent(new FailedToRemoveDropTableEvent(dropTable.Id, ResultStatus.NotFound, [$"no saved entity with id: '{dropTable.Id}'"]));
        }

        private static HandlerTestSetup<RemoveDropTableCommandHandler> Arrange(
            DatabaseState? databaseState = null,
            bool isReadOnlyDatabase = false)
        {
            return new HandlerTestSetup<RemoveDropTableCommandHandler>(
                databaseState ?? DatabaseState.Empty,
                isReadOnlyDatabase,
                ConfigureMocker);
        }

        private static void ConfigureMocker(AutoMocker mocker)
        {
            mocker.Use<IRepository<DropTable>>(new FakeRepository<DropTable>());
        }
    }
}
