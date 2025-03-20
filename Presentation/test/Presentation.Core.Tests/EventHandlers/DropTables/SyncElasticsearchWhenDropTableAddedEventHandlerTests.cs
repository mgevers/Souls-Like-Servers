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
    public class SyncElasticsearchWhenDropTableAddedEventHandlerTests
    {
        [Fact]
        public async Task CanAddEmptyDropTable()
        {
            var monster = DataModelDetails.CreateMonster();
            var dropTable = DataModelDetails.CreateDropTable(monster: monster);

            await Arrange(new DatabaseState(monster))
                .Handle(new DropTableAddedEvent(dropTable.Id, monster.Id, new RollCount(dropTable.RollCount), []))
                .AssertDatabase(new DatabaseState(monster, dropTable));
        }

        [Fact]
        public async Task CanAddPopulatedDropTable()
        {
            var monster = DataModelDetails.CreateMonster();
            var item = DataModelDetails.CreateItem();
            var row = DataModelDetails.CreateDropRateDetail(item: item, dropRate: .5);
            var dropTable = DataModelDetails.CreateDropTable(monster: monster, rows: [row]);

            await Arrange(new DatabaseState(monster, item))
                .Handle(new DropTableAddedEvent(
                    dropTable.Id,
                    monster.Id,
                    new RollCount(dropTable.RollCount),
                    [new KeyValuePair<Guid, DropTableEntry>(row.Id, new DropTableEntry(row.Item.Id, new DropRateDenominator(2)))]))
                .AssertDatabase(new DatabaseState(monster, item, dropTable));
        }

        [Fact]
        public async Task AddPopulatedDropTable_WhenCannotAddToDatabase_ThrowsException()
        {
            var monster = DataModelDetails.CreateMonster();
            var item = DataModelDetails.CreateItem();
            var row = DataModelDetails.CreateDropRateDetail(item: item, dropRate: .5);
            var dropTable = DataModelDetails.CreateDropTable(monster: monster, rows: [row]);

            var dbState = new DatabaseState(monster, item);

            await Arrange(dbState, isReadOnlyDatabase: true)
                .Handle(new DropTableAddedEvent(
                    dropTable.Id,
                    monster.Id,
                    new RollCount(dropTable.RollCount),
                    [new KeyValuePair<Guid, DropTableEntry>(row.Id, new DropTableEntry(row.Item.Id, new DropRateDenominator(2)))]))
                .AssertDatabase(dbState)
                .AssertExceptionThrown();
        }

        [Fact]
        public async Task AddEmptyDropTable_WhenMonsterNotFound_ThrowsException()
        {
            var dropTable = DataModelDetails.CreateDropTable();
            var dbState = DatabaseState.Empty;

            await Arrange(dbState)
                .Handle(new DropTableAddedEvent(dropTable.Id, dropTable.Monster.Id, new RollCount(dropTable.RollCount), []))
                .AssertDatabase(dbState)
                .AssertExceptionThrown();
        }

        [Fact]
        public async Task AddPopulatedDropTable_WhenItemNotFound_ThrowsException()
        {
            var monster = DataModelDetails.CreateMonster();
            var row = DataModelDetails.CreateDropRateDetail();
            var dropTable = DataModelDetails.CreateDropTable(monster: monster, rows: [row]);

            var dbState = new DatabaseState(monster);

            await Arrange(dbState)
                .Handle(new DropTableAddedEvent(
                    dropTable.Id,
                    monster.Id,
                    new RollCount(dropTable.RollCount),
                    [new KeyValuePair<Guid, DropTableEntry>(row.Id, new DropTableEntry(row.Item.Id, new DropRateDenominator(2)))]))
                .AssertDatabase(dbState)
                .AssertExceptionThrown();
        }

        private static HandlerTestSetup<SyncElasticsearchWhenDropTableAddedEventHandler> Arrange(
                DatabaseState? databaseState = null,
                bool isReadOnlyDatabase = false)
        {
            return new HandlerTestSetup<SyncElasticsearchWhenDropTableAddedEventHandler>(
                databaseState ?? DatabaseState.Empty,
                isReadOnlyDatabase,
                ConfigureMocker);
        }

        private static void ConfigureMocker(AutoMocker mocker)
        {
            mocker.Use<IRepository<DropTableDetail>>(new FakeRepository<DropTableDetail>());
            mocker.Use<IRepository<ItemDetail>>(new FakeRepository<ItemDetail>());
            mocker.Use<IRepository<MonsterDetail>>(new FakeRepository<MonsterDetail>());
        }
    }
}
