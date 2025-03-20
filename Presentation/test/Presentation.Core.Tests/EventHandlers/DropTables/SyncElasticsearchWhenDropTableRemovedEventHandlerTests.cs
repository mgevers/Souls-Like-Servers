using Common.Infrastructure.Persistence;
using Common.Testing.FluentTesting;
using Common.Testing.Persistence;
using Monsters.Core.Boundary.Events.DropTables;
using Moq.AutoMock;
using Presentation.Core.DataModels;
using Presentation.Core.EventHandlers.DropTables;
using Presentation.Testing;

namespace Presentation.Core.Tests.EventHandlers.DropTables
{
    public class SyncElasticsearchWhenDropTableRemovedEventHandlerTests
    {
        [Fact]
        public async Task CanRemoveDropTable()
        {
            var monster = DataModelDetails.CreateMonster();
            var dropTable = DataModelDetails.CreateDropTable(monster: monster);

            await Arrange(new DatabaseState(monster, dropTable))
                .Handle(new DropTableRemovedEvent(dropTable.Id))
                .AssertDatabase(new DatabaseState(monster));
        }

        [Fact]
        public async Task RemoveDropTable_WhenTableNotFound_ThrowsException()
        {
            var monster = DataModelDetails.CreateMonster();
            var dropTable = DataModelDetails.CreateDropTable(monster: monster);

            var dbState = new DatabaseState(monster);

            await Arrange(dbState)
                .Handle(new DropTableRemovedEvent(dropTable.Id))
                .AssertDatabase(dbState)
                .AssertExceptionThrown();
        }

        [Fact]
        public async Task RemoveDropTable_WhenCannotRemoveFromDatabase_ThrowsException()
        {
            var monster = DataModelDetails.CreateMonster();
            var dropTable = DataModelDetails.CreateDropTable(monster: monster);

            var dbState = new DatabaseState(monster, dropTable);

            await Arrange(dbState, isReadOnlyDatabase: true)
                .Handle(new DropTableRemovedEvent(dropTable.Id))
                .AssertDatabase(dbState)
                .AssertExceptionThrown();
        }

        private static HandlerTestSetup<SyncElasticsearchWhenDropTableRemovedEventHandler> Arrange(
                DatabaseState? databaseState = null,
                bool isReadOnlyDatabase = false)
        {
            return new HandlerTestSetup<SyncElasticsearchWhenDropTableRemovedEventHandler>(
                databaseState ?? DatabaseState.Empty,
                isReadOnlyDatabase,
                ConfigureMocker);
        }

        private static void ConfigureMocker(AutoMocker mocker)
        {
            mocker.Use<IRepository<DropTableDetail>>(new FakeRepository<DropTableDetail>());
        }
    }
}
