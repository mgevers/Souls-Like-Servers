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
    public class AddRowToDropTableCommandHandlerTests
    {
        [Fact]
        public async Task CanAddRowToDropTable()
        {
            var dropTable = Entities.CreateEmptyDropTable();
            var item = Entities.CreateItem();
            var row = Entities.CreateDropTableRow(item: item);

            var endTable = JsonConvert.DeserializeObject<DropTable>(JsonConvert.SerializeObject(dropTable))!;
            endTable.AddRow(row);

            await Arrange(new DatabaseState(dropTable, item))
                .Handle(new AddRowToDropTableCommand(dropTable.Id, row.Id, new DropTableEntry(row.Item.Id, row.DropRateDenominator)))
                .AssertDatabase(new DatabaseState(endTable, item))
                .AssertPublishedEvent(new RowAddedToDropTableEvent(dropTable.Id, new KeyValuePair<Guid, DropTableEntry>(row.Id, new DropTableEntry(row.Item.Id, row.DropRateDenominator))));
        }

        [Fact]
        public async Task AddRow_WhenCannotAddToDatabase_PublishesFailure()
        {
            var dropTable = Entities.CreateEmptyDropTable();
            var item = Entities.CreateItem();
            var row = Entities.CreateDropTableRow(item: item);
            var dbState = new DatabaseState(dropTable, item);

            await Arrange(dbState, isReadOnlyDatabase: true)
                .Handle(new AddRowToDropTableCommand(dropTable.Id, row.Id, new DropTableEntry(row.Item.Id, row.DropRateDenominator)))
                .AssertDatabase(dbState)
                .AssertPublishedEvent(new FailedToAddRowToDropTableEvent(dropTable.Id, row.Id, ResultStatus.CriticalError, ["cannot write to readonly database"]));
        }

        [Fact]
        public async Task AddRow_WhenTableAlreadyFull_PublishesFailure()
        {
            var existingItems = new Item[]
            {
                Entities.CreateItem(),
                Entities.CreateItem(),
                Entities.CreateItem(),
            };
            var dropTable = Entities.CreateDropTable([
                Entities.CreateDropTableRow(item: existingItems[0], dropRate: new DropRateDenominator(2)),
                Entities.CreateDropTableRow(item: existingItems[1], dropRate: new DropRateDenominator(2)),
            ]);
            var row = Entities.CreateDropTableRow(item: existingItems[2]);

            var dbState = new DatabaseState(dropTable, existingItems[0], existingItems[1], existingItems[2]);

            await Arrange(dbState)
                .Handle(new AddRowToDropTableCommand(dropTable.Id, row.Id, new DropTableEntry(row.Item.Id, row.DropRateDenominator)))
                .AssertDatabase(dbState)
                .AssertPublishedEvent(new FailedToAddRowToDropTableEvent(dropTable.Id, row.Id, ResultStatus.Invalid, ["cannot add row, would make total drop rate greater than 100%"]));
        }

        private static HandlerTestSetup<AddRowToDropTableCommandHandler> Arrange(
            DatabaseState? databaseState = null,
            bool isReadOnlyDatabase = false)
        {
            return new HandlerTestSetup<AddRowToDropTableCommandHandler>(
                databaseState ?? DatabaseState.Empty,
                isReadOnlyDatabase,
                ConfigureMocker);
        }

        private static void ConfigureMocker(AutoMocker mocker)
        {
            mocker.Use<IRepository<DropTable>>(new FakeRepository<DropTable>());
            mocker.Use<IRepository<Item>>(new FakeRepository<Item>());
        }
    }
}
