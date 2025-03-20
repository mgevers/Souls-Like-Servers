using Common.Infrastructure.Persistence;
using Common.Testing.FluentTesting;
using Common.Testing.Persistence;
using Monsters.Core.Boundary.Events.Monsters;
using Moq.AutoMock;
using Presentation.Core.DataModels;
using Presentation.Core.EventHandlers.Monsters;
using Presentation.Testing;

namespace Presentation.Core.Tests.EventHandlers.Monsters
{
    public class SyncElasticsearchWhenMonsterRemovedEventHandlerTests
    {
        [Fact]
        public async Task CanRemoveMonster()
        {
            var monster = DataModelDetails.CreateMonster();

            await Arrange(new DatabaseState(monster))
                .Handle(new MonsterRemovedEvent(monster.Id))
                .AssertDatabase(DatabaseState.Empty);
        }

        [Fact]
        public async Task RemoveMonster_WhenCannotRemoveFromDatabase_ThrowsException()
        {
            var monster = DataModelDetails.CreateMonster();
            var dbState = new DatabaseState(monster);

            await Arrange(dbState, isReadOnlyDatabase: true)
                .Handle(new MonsterRemovedEvent(monster.Id))
                .AssertDatabase(dbState)
                .AssertExceptionThrown();
        }

        private static HandlerTestSetup<SyncElasticsearchWhenMonsterRemovedEventHandler> Arrange(
                DatabaseState? databaseState = null,
                bool isReadOnlyDatabase = false)
        {
            return new HandlerTestSetup<SyncElasticsearchWhenMonsterRemovedEventHandler>(
                databaseState ?? DatabaseState.Empty,
                isReadOnlyDatabase,
                ConfigureMocker);
        }

        private static void ConfigureMocker(AutoMocker mocker)
        {
            mocker.Use<IRepository<MonsterDetail>>(new FakeRepository<MonsterDetail>());
        }
    }
}
