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
    public class UpdateDropTableRollCountCommandHandlerTests
    {
        [Fact]
        public async Task CanUpdateRollCount()
        {
            var dropTable = Entities.CreateEmptyDropTable();
            var newRollCount = new RollCount(2);

            var endTable = JsonConvert.DeserializeObject<DropTable>(JsonConvert.SerializeObject(dropTable))!;
            endTable.RollCount = newRollCount;

            await Arrange(new DatabaseState(dropTable))
                .Handle(new UpdateDropTableRollCountCommand(dropTable.Id, newRollCount))
                .AssertDatabase(new DatabaseState(endTable))
                .AssertPublishedEvent(new DropTableRollCountUpdatedEvent(dropTable.Id, newRollCount));
        }

        [Fact]
        public async Task UpdateRollCount_WhenCannotAddToDatabase_PublishesFailure()
        {
            var dropTable = Entities.CreateEmptyDropTable();
            var newRollCount = new RollCount(2);
            var dbState = new DatabaseState(dropTable);

            await Arrange(dbState, isReadOnlyDatabase: true)
                .Handle(new UpdateDropTableRollCountCommand(dropTable.Id, newRollCount))
                .AssertDatabase(dbState)
                .AssertPublishedEvent(new FailedToUpdateDropTableRollCountEvent(dropTable.Id, newRollCount, ResultStatus.CriticalError, ["cannot write to readonly database"]));
        }

        [Fact]
        public async Task UpdateRollCount_WhenTableDoesNotExist_PublishesFailure()
        {
            var dropTable = Entities.CreateEmptyDropTable();
            var newRollCount = new RollCount(2);
            var dbState = DatabaseState.Empty;

            await Arrange(dbState)
                .Handle(new UpdateDropTableRollCountCommand(dropTable.Id, newRollCount))
                .AssertDatabase(dbState)
                .AssertPublishedEvent(new FailedToUpdateDropTableRollCountEvent(dropTable.Id, newRollCount, ResultStatus.NotFound, [$"no saved entity with id: '{dropTable.Id}'"]));
        }

        private static HandlerTestSetup<UpdateDropTableRollCountCommandHandler> Arrange(
            DatabaseState? databaseState = null,
            bool isReadOnlyDatabase = false)
        {
            return new HandlerTestSetup<UpdateDropTableRollCountCommandHandler>(
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
