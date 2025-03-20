using Common.Infrastructure.Persistence;
using Common.Testing.FluentTesting;
using Common.Testing.Persistence;
using Monsters.Core.Boundary.Events.DropTables;
using Monsters.Core.Boundary.ValueObjects;
using Moq.AutoMock;
using Presentation.Core.DataModels;
using Presentation.Core.EventHandlers.DropTables;
using Presentation.Testing;

namespace Presentation.Core.Tests.EventHandlers.DropTables
{
    public class SyncElasticsearchWhenDropTableUpdatedEventHandlerTests
    {
        [Fact]
        public async Task CanUpdateTableRollCount()
        {
            var monster = DataModelDetails.CreateMonster();
            var dropTable = DataModelDetails.CreateDropTable(monster: monster);
            var updatedTable = DataModelDetails.CreateDropTable(id: dropTable.Id, monster: monster, rollCount: 4);

            await Arrange(new DatabaseState(monster, dropTable))
                .Handle(new DropTableRollCountUpdatedEvent(dropTable.Id, new RollCount(updatedTable.RollCount)))
                .AssertDatabase(new DatabaseState(monster, updatedTable));
        }

        [Fact]
        public async Task UpdateTableRollCount_WhenTableNotFound_ThrowsException()
        {
            var monster = DataModelDetails.CreateMonster();
            var dropTable = DataModelDetails.CreateDropTable(monster: monster);

            var dbState = new DatabaseState(monster);

            await Arrange(dbState)
                .Handle(new DropTableRollCountUpdatedEvent(dropTable.Id, new RollCount(4)))
                .AssertDatabase(dbState)
                .AssertExceptionThrown();
        }

        [Fact]
        public async Task UpdateTableRollCount_WhenCannotUpdateDatabase_ThrowsException()
        {
            var monster = DataModelDetails.CreateMonster();
            var dropTable = DataModelDetails.CreateDropTable(monster: monster);

            var dbState = new DatabaseState(monster, dropTable);

            await Arrange(dbState, isReadOnlyDatabase: true)
                .Handle(new DropTableRollCountUpdatedEvent(dropTable.Id, new RollCount(4)))
                .AssertDatabase(dbState)
                .AssertExceptionThrown();
        }

        [Fact]
        public async Task CanAddRowToDropTable()
        {
            var monster = DataModelDetails.CreateMonster();
            var dropTable = DataModelDetails.CreateDropTable(monster: monster);

            var item = DataModelDetails.CreateItem();
            var row = DataModelDetails.CreateDropRateDetail(item: item, dropRate: .5);
            var updatedTable = DataModelDetails.CreateDropTable(id: dropTable.Id, monster: monster, rows: [row]);

            await Arrange(new DatabaseState(monster, item, dropTable))
                .Handle(new RowAddedToDropTableEvent(
                    dropTable.Id,
                    new KeyValuePair<Guid, DropTableEntry>(row.Id, new DropTableEntry(item.Id, new DropRateDenominator(2)))))
                .AssertDatabase(new DatabaseState(monster, item, updatedTable));
        }

        [Fact]
        public async Task AddRowToDropTable_WhenTableNotFound_ThrowsException()
        {
            var monster = DataModelDetails.CreateMonster();
            var dropTable = DataModelDetails.CreateDropTable(monster: monster);

            var dbState = new DatabaseState(monster);

            await Arrange(dbState)
                .Handle(new RowAddedToDropTableEvent(
                    dropTable.Id,
                    new KeyValuePair<Guid, DropTableEntry>(Guid.NewGuid(), new DropTableEntry(Guid.NewGuid(), new DropRateDenominator(2)))))
                .AssertDatabase(dbState)
                .AssertExceptionThrown();
        }

        [Fact]
        public async Task AddRowToDropTable_WhenItemNotFound_ThrowsException()
        {
            var monster = DataModelDetails.CreateMonster();
            var dropTable = DataModelDetails.CreateDropTable(monster: monster);

            var dbState = new DatabaseState(monster, dropTable);

            await Arrange(dbState)
                .Handle(new RowAddedToDropTableEvent(
                    dropTable.Id,
                    new KeyValuePair<Guid, DropTableEntry>(Guid.NewGuid(), new DropTableEntry(Guid.NewGuid(), new DropRateDenominator(2)))))
                .AssertDatabase(dbState)
                .AssertExceptionThrown();
        }

        [Fact]
        public async Task AddRowToDropTable_WhenCannotUpdateDatabase_ThrowsException()
        {
            var monster = DataModelDetails.CreateMonster();
            var dropTable = DataModelDetails.CreateDropTable(monster: monster);

            var item = DataModelDetails.CreateItem();
            var row = DataModelDetails.CreateDropRateDetail(item: item, dropRate: .5);
            var updatedTable = DataModelDetails.CreateDropTable(id: dropTable.Id, monster: monster, rows: [row]);

            var dbState = new DatabaseState(monster, item, dropTable);

            await Arrange(dbState, isReadOnlyDatabase: true)
                .Handle(new RowAddedToDropTableEvent(
                    dropTable.Id,
                    new KeyValuePair<Guid, DropTableEntry>(row.Id, new DropTableEntry(item.Id, new DropRateDenominator(2)))))
                .AssertDatabase(dbState)
                .AssertExceptionThrown();
        }

        [Fact]
        public async Task CanRemoveRowFromDropTable()
        {
            var monster = DataModelDetails.CreateMonster();
            var item = DataModelDetails.CreateItem();
            var row = DataModelDetails.CreateDropRateDetail(item: item, dropRate: .5);
            var dropTable = DataModelDetails.CreateDropTable(monster: monster, rows: [row]);
            var updatedTable = DataModelDetails.CreateDropTable(id: dropTable.Id, monster: monster, rows: []);

            await Arrange(new DatabaseState(monster, item, dropTable))
                .Handle(new RowRemovedFromDropTableEvent(dropTable.Id, row.Id))
                .AssertDatabase(new DatabaseState(monster, item, updatedTable));
        }

        [Fact]
        public async Task RemoveRowFromDropTable_WhenRowNotFound_ThrowsException()
        {
            var monster = DataModelDetails.CreateMonster();
            var item = DataModelDetails.CreateItem();
            var row = DataModelDetails.CreateDropRateDetail(item: item, dropRate: .5);
            var dropTable = DataModelDetails.CreateDropTable(monster: monster, rows: [row]);

            var dbState = new DatabaseState(monster, item, dropTable);

            await Arrange(dbState)
                .Handle(new RowRemovedFromDropTableEvent(dropTable.Id, Guid.NewGuid()))
                .AssertDatabase(dbState)
                .AssertExceptionThrown();
        }

        [Fact]
        public async Task RemoveRowFromDropTable_WhenTableNotFound_ThrowsException()
        {
            var monster = DataModelDetails.CreateMonster();
            var item = DataModelDetails.CreateItem();

            var dbState = new DatabaseState(monster, item);

            await Arrange(dbState)
                .Handle(new RowRemovedFromDropTableEvent(tableId: Guid.NewGuid(), rowId: Guid.NewGuid()))
                .AssertDatabase(dbState)
                .AssertExceptionThrown();
        }

        [Fact]
        public async Task RemoveRowFromDropTable_WhenCannotUpdateDatabase_ThrowsException()
        {
            var monster = DataModelDetails.CreateMonster();
            var item = DataModelDetails.CreateItem();
            var row = DataModelDetails.CreateDropRateDetail(item: item, dropRate: .5);
            var dropTable = DataModelDetails.CreateDropTable(monster: monster, rows: [row]);

            var dbState = new DatabaseState(monster, item, dropTable);

            await Arrange(dbState, isReadOnlyDatabase: true)
                .Handle(new RowRemovedFromDropTableEvent(dropTable.Id, row.Id))
                .AssertDatabase(dbState)
                .AssertExceptionThrown();
        }

        private static HandlerTestSetup<SyncElasticsearchWhenDropTableUpdatedEventHandler> Arrange(
                DatabaseState? databaseState = null,
                bool isReadOnlyDatabase = false)
        {
            return new HandlerTestSetup<SyncElasticsearchWhenDropTableUpdatedEventHandler>(
                databaseState ?? DatabaseState.Empty,
                isReadOnlyDatabase,
                ConfigureMocker);
        }

        private static void ConfigureMocker(AutoMocker mocker)
        {
            mocker.Use<IRepository<DropTableDetail>>(new FakeRepository<DropTableDetail>());
            mocker.Use<IRepository<ItemDetail>>(new FakeRepository<ItemDetail>());
        }
    }
}
