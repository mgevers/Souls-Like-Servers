using Ardalis.Result;
using Common.Core.Boundary;
using Common.Infrastructure.Persistence;
using Common.Testing.FluentTesting;
using Common.Testing.Persistence;
using Monsters.Core.Boundary.Events.Monsters;
using Monsters.Core.CommandHandlers.Monsters;
using Monsters.Core.Commands.Monsters;
using Monsters.Core.Domain;
using Monsters.Testing;
using Moq.AutoMock;
using Newtonsoft.Json;

namespace Monsters.Core.Tests.CommandHandlers.Monsters
{
    public class UpdateMonsterAttributeSetCommandHandlerTests
    {
        [Fact]
        public async Task CanUpdateMonsterAttributeSet()
        {
            var monster = Entities.CreateMonster();
            var newAttributes = new SoulsAttributeSet(physicalDefense: 100);

            var endMonster = JsonConvert.DeserializeObject<Monster>(JsonConvert.SerializeObject(monster))!;
            endMonster.AttributeSet = newAttributes;

            await Arrange(new DatabaseState(monster))
                .Handle(new UpdateMonsterAttributeSetCommand(monster.Id, newAttributes))
                .AssertDatabase(new DatabaseState(endMonster))
                .AssertPublishedEvent(new MonsterAttributeSetUpdatedEvent(monster.Id, newAttributes));
        }

        [Fact]
        public async Task UpdateMonsterAttributeSet_WhenCannotAddToDatabase_PublishesFailure()
        {
            var monster = Entities.CreateMonster();
            var dbState = new DatabaseState(monster);
            var newAttributes = new SoulsAttributeSet(physicalDefense: 100);

            await Arrange(dbState, isReadOnlyDatabase: true)
                .Handle(new UpdateMonsterAttributeSetCommand(monster.Id, newAttributes))
                .AssertDatabase(dbState)
                .AssertPublishedEvent(new FailedToUpdateMonsterAttributeSetEvent(monster.Id, newAttributes, ResultStatus.CriticalError, ["cannot write to readonly database"]));
        }

        [Fact]
        public async Task UpdateMonsterAttributeSet_WhenMonsterDoesNotExist_PublishesFailure()
        {
            var monster = Entities.CreateMonster();
            var dbState = DatabaseState.Empty;
            var newAttributes = new SoulsAttributeSet(physicalDefense: 100);

            await Arrange(dbState)
                .Handle(new UpdateMonsterAttributeSetCommand(monster.Id, newAttributes))
                .AssertDatabase(dbState)
                .AssertPublishedEvent(new FailedToUpdateMonsterAttributeSetEvent(monster.Id, newAttributes, ResultStatus.NotFound, [$"no saved entity with id: '{monster.Id}'"]));
        }

        private static HandlerTestSetup<UpdateMonsterAttributeSetCommandHandler> Arrange(
                DatabaseState? databaseState = null,
                bool isReadOnlyDatabase = false)
        {
            return new HandlerTestSetup<UpdateMonsterAttributeSetCommandHandler>(
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
