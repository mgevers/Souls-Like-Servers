using Common.Infrastructure.Persistence;
using Common.Testing.FluentTesting;
using Common.Testing.Persistence;
using Monsters.Core.Boundary.Events.Monsters;
using Monsters.Core.Boundary.ValueObjects;
using Moq.AutoMock;
using Presentation.Core.DataModels;
using Presentation.Core.EventHandlers.Monsters;
using Presentation.Testing;

namespace Presentation.Core.Tests.EventHandlers.Monsters
{
    public class SyncElasticsearchWhenMonsterAddedEventHandlerTests
    {
        [Fact]
        public async Task CanAddMonster()
        {
            var monster = DataModelDetails.CreateMonster();

            await Arrange()
                .Handle(new MonsterAddedEvent(
                    monster.Id,
                    new MonsterName(monster.MonsterName),
                    new MonsterLevel(monster.MonsterLevel), monster.AttributeSet))
                .AssertDatabase(new DatabaseState(monster));
        }

        [Fact]
        public async Task AddMonster_WhenCannotAddToDatabase_ThrowsException()
        {
            var monster = DataModelDetails.CreateMonster();
            var dbState = DatabaseState.Empty;

            await Arrange(dbState, isReadOnlyDatabase: true)
                .Handle(new MonsterAddedEvent(
                    monster.Id,
                    new MonsterName(monster.MonsterName),
                    new MonsterLevel(monster.MonsterLevel), monster.AttributeSet))
                .AssertDatabase(dbState)
                .AssertExceptionThrown();
        }

        private static HandlerTestSetup<SyncElasticsearchWhenMonsterAddedEventHandler> Arrange(
                DatabaseState? databaseState = null,
                bool isReadOnlyDatabase = false)
        {
            return new HandlerTestSetup<SyncElasticsearchWhenMonsterAddedEventHandler>(
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
