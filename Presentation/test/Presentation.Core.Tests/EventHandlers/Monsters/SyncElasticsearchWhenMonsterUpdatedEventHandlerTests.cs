using Common.Core.Boundary;
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
    public class SyncElasticsearchWhenMonsterUpdatedEventHandlerTests
    {
        [Fact]
        public async Task CanUpdateMonsterAttributeSet()
        {
            var monster = DataModelDetails.CreateMonster();
            var updatedMonster = DataModelDetails.CreateMonster(
                id: monster.Id,
                attributeSet: new SoulsAttributeSet(physicalPower: 10));

            await Arrange(new DatabaseState(monster))
                .Handle(new MonsterAttributeSetUpdatedEvent(monster.Id, updatedMonster.AttributeSet))
                .AssertDatabase(new DatabaseState(updatedMonster));
        }

        [Fact]
        public async Task UpdateMonsterAttributeSet_WhenCannotModifyDatabase_ThrowsException()
        {
            var monster = DataModelDetails.CreateMonster();
            var updatedMonster = DataModelDetails.CreateMonster(
                id: monster.Id,
                attributeSet: new SoulsAttributeSet(physicalPower: 10));
            var dbState = new DatabaseState(monster);

            await Arrange(dbState, isReadOnlyDatabase: true)
                .Handle(new MonsterAttributeSetUpdatedEvent(monster.Id, updatedMonster.AttributeSet))
                .AssertDatabase(dbState)
                .AssertExceptionThrown();
        }

        [Fact]
        public async Task CanUpdateMonsterName()
        {
            var monster = DataModelDetails.CreateMonster();
            var updatedMonster = DataModelDetails.CreateMonster(
                id: monster.Id,
                monsterName: "Skeleton");

            await Arrange(new DatabaseState(monster))
                .Handle(new MonsterNameUpdatedEvent(monster.Id, new MonsterName(updatedMonster.MonsterName)))
                .AssertDatabase(new DatabaseState(updatedMonster));
        }

        [Fact]
        public async Task UpdateMonsterName_WhenCannotModifyDatabase_ThrowsException()
        {
            var monster = DataModelDetails.CreateMonster();
            var updatedMonster = DataModelDetails.CreateMonster(
                id: monster.Id,
                monsterName: "Skeleton");
            var dbState = new DatabaseState(monster);

            await Arrange(dbState, isReadOnlyDatabase: true)
                .Handle(new MonsterNameUpdatedEvent(monster.Id, new MonsterName(updatedMonster.MonsterName)))
                .AssertDatabase(dbState)
                .AssertExceptionThrown();
        }

        [Fact]
        public async Task CanUpdateMonsterLevel()
        {
            var monster = DataModelDetails.CreateMonster();
            var updatedMonster = DataModelDetails.CreateMonster(
                id: monster.Id,
                monsterLevel: 10);

            await Arrange(new DatabaseState(monster))
                .Handle(new MonsterLevelUpdatedEvent(monster.Id, new MonsterLevel(updatedMonster.MonsterLevel)))
                .AssertDatabase(new DatabaseState(updatedMonster));
        }

        [Fact]
        public async Task UpdateMonsterLevel_WhenCannotModifyDatabase_ThrowsException()
        {
            var monster = DataModelDetails.CreateMonster();
            var updatedMonster = DataModelDetails.CreateMonster(
                id: monster.Id,
                monsterLevel: 10);
            var dbState = new DatabaseState(monster);

            await Arrange(dbState, isReadOnlyDatabase: true)
                .Handle(new MonsterLevelUpdatedEvent(monster.Id, new MonsterLevel(updatedMonster.MonsterLevel)))
                .AssertDatabase(dbState)
                .AssertExceptionThrown();
        }

        private static HandlerTestSetup<SyncElasticsearchWhenMonsterUpdatedEventHandler> Arrange(
                DatabaseState? databaseState = null,
                bool isReadOnlyDatabase = false)
        {
            return new HandlerTestSetup<SyncElasticsearchWhenMonsterUpdatedEventHandler>(
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
