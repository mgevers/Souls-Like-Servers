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
    public class UpdateMonsterNameCommandHandlerTests
    {
        [Fact]
        public async Task CanUpdateMonsterName()
        {
            var monster = Entities.CreateMonster();
            var monsterName = new MonsterName("Zombie");

            var endMonster = JsonConvert.DeserializeObject<Monster>(JsonConvert.SerializeObject(monster))!;
            endMonster.Name = monsterName;

            await Arrange(new DatabaseState(monster))
                .Handle(new UpdateMonsterNameCommand(monster.Id, monsterName))
                .AssertDatabase(new DatabaseState(endMonster))
                .AssertPublishedEvent(new MonsterNameUpdatedEvent(monster.Id, monsterName));
        }

        [Fact]
        public async Task UpdateMonsterName_WhenCannotAddToDatabase_PublishesFailure()
        {
            var monster = Entities.CreateMonster();
            var dbState = new DatabaseState(monster);
            var monsterName = new MonsterName("Zombie");

            await Arrange(dbState, isReadOnlyDatabase: true)
                .Handle(new UpdateMonsterNameCommand(monster.Id, monsterName))
                .AssertDatabase(dbState)
                .AssertPublishedEvent(new FailedToUpdateMonsterNameEvent(monster.Id, monsterName, ResultStatus.CriticalError, ["cannot write to readonly database"]));
        }

        [Fact]
        public async Task UpdateMonsterName_WhenMonsterDoesNotExist_PublishesFailure()
        {
            var monster = Entities.CreateMonster();
            var dbState = DatabaseState.Empty;
            var monsterName = new MonsterName("Zombie");

            await Arrange(dbState)
                .Handle(new UpdateMonsterNameCommand(monster.Id, monsterName))
                .AssertDatabase(dbState)
                .AssertPublishedEvent(new FailedToUpdateMonsterNameEvent(monster.Id, monsterName, ResultStatus.NotFound, [$"no saved entity with id: '{monster.Id}'"]));
        }

        private static HandlerTestSetup<UpdateMonsterNameCommandHandler> Arrange(
                DatabaseState? databaseState = null,
                bool isReadOnlyDatabase = false)
        {
            return new HandlerTestSetup<UpdateMonsterNameCommandHandler>(
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
