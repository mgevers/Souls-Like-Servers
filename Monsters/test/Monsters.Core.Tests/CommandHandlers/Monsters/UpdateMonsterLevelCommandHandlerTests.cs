using Ardalis.Result;
using Common.Infrastructure.Persistence;
using Common.Testing.FluentTesting;
using Common.Testing.Persistence;
using Monsters.Core.Boundary.Events.Monsters;
using Monsters.Core.Boundary.ValueObjects;
using Monsters.Core.CommandHandlers.Monsters;
using Monsters.Core.Commands.Monsters;
using Monsters.Core.Domain;
using Monsters.Testing;
using Moq.AutoMock;
using Newtonsoft.Json;

namespace Monsters.Core.Tests.CommandHandlers.Monsters
{
    public class UpdateMonsterLevelCommandHandlerTests
    {
        [Fact]
        public async Task CanUpdateMonsterLevel()
        {
            var monster = Entities.CreateMonster();
            var monsterLevel = new MonsterLevel(10);

            var endMonster = JsonConvert.DeserializeObject<Monster>(JsonConvert.SerializeObject(monster))!;
            endMonster.Level = monsterLevel;

            await Arrange(new DatabaseState(monster))
                .Handle(new UpdateMonsterLevelCommand(monster.Id, monsterLevel))
                .AssertDatabase(new DatabaseState(endMonster))
                .AssertPublishedEvent(new MonsterLevelUpdatedEvent(monster.Id, monsterLevel));
        }

        [Fact]
        public async Task UpdateMonsterLevel_WhenCannotAddToDatabase_PublishesFailure()
        {
            var monster = Entities.CreateMonster();
            var dbState = new DatabaseState(monster);
            var monsterLevel = new MonsterLevel(10);

            await Arrange(dbState, isReadOnlyDatabase: true)
                .Handle(new UpdateMonsterLevelCommand(monster.Id, monsterLevel))
                .AssertDatabase(dbState)
                .AssertPublishedEvent(new FailedToUpdateMonsterLevelEvent(monster.Id, monsterLevel, ResultStatus.CriticalError, ["cannot write to readonly database"]));
        }

        [Fact]
        public async Task UpdateMonsterLevel_WhenMonsterDoesNotExist_PublishesFailure()
        {
            var monster = Entities.CreateMonster();
            var dbState = DatabaseState.Empty;
            var monsterLevel = new MonsterLevel(10);

            await Arrange(dbState)
                .Handle(new UpdateMonsterLevelCommand(monster.Id, monsterLevel))
                .AssertDatabase(dbState)
                .AssertPublishedEvent(new FailedToUpdateMonsterLevelEvent(monster.Id, monsterLevel, ResultStatus.NotFound, [$"no saved entity with id: '{monster.Id}'"]));
        }

        private static HandlerTestSetup<UpdateMonsterLevelCommandHandler> Arrange(
                DatabaseState? databaseState = null,
                bool isReadOnlyDatabase = false)
        {
            return new HandlerTestSetup<UpdateMonsterLevelCommandHandler>(
                databaseState ?? DatabaseState.Empty,
                isReadOnlyDatabase,
                ConfigureMocker);
        }

        private static void ConfigureMocker(AutoMocker mocker)
        {
            mocker.Use<IRepository<Monster>>(new FakeRepository<Monster>());
        }
    }
}
