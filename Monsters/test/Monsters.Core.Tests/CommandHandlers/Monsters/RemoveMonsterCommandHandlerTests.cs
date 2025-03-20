using Ardalis.Result;
using Common.Infrastructure.Persistence;
using Common.Testing.FluentTesting;
using Common.Testing.Persistence;
using Monsters.Core.Boundary.Events.Monsters;
using Monsters.Core.CommandHandlers.Monsters;
using Monsters.Core.Commands.Monsters;
using Monsters.Core.Domain;
using Monsters.Testing;
using Moq.AutoMock;

namespace Monsters.Core.Tests.CommandHandlers.Monsters
{
    public class RemoveMonsterCommandHandlerTests
    {
        [Fact]
        public async Task CanRemoveMonster()
        {
            var monster = Entities.CreateMonster();

            await Arrange(new DatabaseState(monster))
                .Handle(new RemoveMonsterCommand(monster.Id))
                .AssertDatabase(DatabaseState.Empty)
                .AssertPublishedEvent(new MonsterRemovedEvent(monster.Id));
        }

        [Fact]
        public async Task RemoveMonster_WhenCannotAddToDatabase_PublishesFailure()
        {
            var monster = Entities.CreateMonster();
            var dbState = new DatabaseState(monster);

            await Arrange(dbState, isReadOnlyDatabase: true)
                .Handle(new RemoveMonsterCommand(monster.Id))
                .AssertDatabase(dbState)
                .AssertPublishedEvent(new FailedToRemoveMonsterEvent(monster.Id, ResultStatus.CriticalError, ["cannot write to readonly database"]));
        }

        [Fact]
        public async Task RemoveMonster_WhenMonsterDoesNotExist_PublishesFailure()
        {
            var monster = Entities.CreateMonster();
            var dbState = DatabaseState.Empty;

            await Arrange(dbState)
                .Handle(new RemoveMonsterCommand(monster.Id))
                .AssertDatabase(dbState)
                .AssertPublishedEvent(new FailedToRemoveMonsterEvent(monster.Id, ResultStatus.NotFound, [$"no saved entity with id: '{monster.Id}'"]));
        }

        private static HandlerTestSetup<RemoveMonsterCommandHandler> Arrange(
                DatabaseState? databaseState = null,
                bool isReadOnlyDatabase = false)
        {
            return new HandlerTestSetup<RemoveMonsterCommandHandler>(
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
