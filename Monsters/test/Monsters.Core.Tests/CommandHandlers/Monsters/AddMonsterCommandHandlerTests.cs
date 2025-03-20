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
    public class AddMonsterCommandHandlerTests
    {
        [Fact]
        public async Task CanAddMonster()
        {
            var monster = Entities.CreateMonster();

            await Arrange()
                .Handle(new AddMonsterCommand(monster.Id, monster.Name, monster.Level, monster.AttributeSet))
                .AssertDatabase(new DatabaseState(monster))
                .AssertPublishedEvent(new MonsterAddedEvent(monster.Id, monster.Name, monster.Level, monster.AttributeSet));
        }

        [Fact]
        public async Task AddMonster_WhenCannotAddToDatabase_PublishesFailure()
        {
            var monster = Entities.CreateMonster();

            await Arrange(isReadOnlyDatabase: true)
                .Handle(new AddMonsterCommand(monster.Id, monster.Name, monster.Level, monster.AttributeSet))
                .AssertDatabase(DatabaseState.Empty)
                .AssertPublishedEvent(new FailedToAddMonsterEvent(monster.Id, ResultStatus.CriticalError, ["cannot write to readonly database"]));
        }

        [Fact]
        public async Task AddMonster_WhenMonsterAreadyExists_PublishesFailure()
        {
            var monster = Entities.CreateMonster();
            var dbState = new DatabaseState(monster);

            await Arrange(dbState)
                .Handle(new AddMonsterCommand(monster.Id, monster.Name, monster.Level, monster.AttributeSet))
                .AssertDatabase(dbState)
                .AssertPublishedEvent(new FailedToAddMonsterEvent(monster.Id, ResultStatus.Conflict, [$"conflict - entity with id {monster.Id} already exists"]));
        }

        private static HandlerTestSetup<AddMonsterCommandHandler> Arrange(
                DatabaseState? databaseState = null,
                bool isReadOnlyDatabase = false)
        {
            return new HandlerTestSetup<AddMonsterCommandHandler>(
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
