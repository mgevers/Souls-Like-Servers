using Ardalis.Result;
using Common.Infrastructure.Persistence;
using Common.Testing.FluentTesting;
using Common.Testing.Persistence;
using Monsters.Core.Boundary.Events.DropTables;
using Monsters.Core.Boundary.ValueObjects;
using Monsters.Core.CommandHandlers.DropTables;
using Monsters.Core.Commands.DropTables;
using Monsters.Core.Domain;
using Monsters.Testing;
using Moq.AutoMock;
using Newtonsoft.Json;

namespace Monsters.Core.Tests.CommandHandlers.DropTables
{
    public class RemoveRowFromDropTableCommandHandlerTests
    {
        [Fact]
        public async Task CanRemoveRowFromDropTable()
        {
            var row = Entities.CreateDropTableRow();
            var dropTable = Entities.CreateDropTable([
                row,
            ]);

            var endTable = JsonConvert.DeserializeObject<DropTable>(JsonConvert.SerializeObject(dropTable))!;
            endTable.RemoveRow(row.Id);

            await Arrange(new DatabaseState(dropTable))
                .Handle(new RemoveRowFromDropTableCommand(dropTable.Id, row.Id))
                .AssertDatabase(new DatabaseState(endTable))
                .AssertPublishedEvent(new RowRemovedFromDropTableEvent(dropTable.Id, row.Id));
        }

        [Fact]
        public async Task RemoveRow_WhenCannotWriteToDatabase_PublishesFailure()
        {
            var row = Entities.CreateDropTableRow();
            var dropTable = Entities.CreateDropTable([
                row,
            ]);
            var dbState = new DatabaseState(dropTable);

            await Arrange(dbState, isReadOnlyDatabase: true)
                .Handle(new RemoveRowFromDropTableCommand(dropTable.Id, row.Id))
                .AssertDatabase(dbState)
                .AssertPublishedEvent(new FailedToRemoveRowFromDropTableEvent(dropTable.Id, row.Id, ResultStatus.CriticalError, ["cannot write to readonly database"]));
        }

        [Fact]
        public async Task RemoveRow_WhenTableHasNoMatchingRow_ThrowsException()
        {
            var dropTable = Entities.CreateDropTable([
                Entities.CreateDropTableRow(dropRate: new DropRateDenominator(2)),
                Entities.CreateDropTableRow(dropRate: new DropRateDenominator(2)),
            ]);
            var rowId = Guid.NewGuid();
            var dbState = new DatabaseState(dropTable);

            await Arrange(dbState)
                .Handle(new RemoveRowFromDropTableCommand(dropTable.Id, rowId))
                .AssertDatabase(dbState)
                .AssertExceptionThrown();
        }

        private static HandlerTestSetup<RemoveRowFromDropTableCommandHandler> Arrange(
            DatabaseState? databaseState = null,
            bool isReadOnlyDatabase = false)
        {
            return new HandlerTestSetup<RemoveRowFromDropTableCommandHandler>(
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
