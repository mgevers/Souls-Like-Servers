using Ardalis.Result;
using Common.Core.Boundary;
using Common.Infrastructure.Persistence;
using Common.Testing.FluentTesting;
using Common.Testing.Persistence;
using Monsters.Core.Boundary.Events.Items;
using Monsters.Core.CommandHandlers.Items;
using Monsters.Core.Commands.Items;
using Monsters.Core.Domain;
using Monsters.Testing;
using Moq.AutoMock;
using Newtonsoft.Json;

namespace Monsters.Core.Tests.CommandHandlers.Items
{
    public class UpdateItemAttributeSetCommandHandlerTests
    {
        [Fact]
        public async Task CanUpdateItemAttributeSet()
        {
            var item = Entities.CreateItem();
            var newAttributes = new SoulsAttributeSet(physicalPower: 100);

            var endItem = JsonConvert.DeserializeObject<Item>(JsonConvert.SerializeObject(item))!;
            endItem.AttributeSet = newAttributes;

            await Arrange(new DatabaseState(item))
                .Handle(new UpdateItemAttributeSetCommand(item.Id, newAttributes))
                .AssertDatabase(new DatabaseState(endItem))
                .AssertPublishedEvent(new ItemAttributeSetUpdatedEvent(item.Id, newAttributes));
        }

        [Fact]
        public async Task UpdateItemAttributeSet_WhenCannotRemoveFromDatabase_PublishesFailure()
        {
            var item = Entities.CreateItem();
            var dbState = new DatabaseState(item);
            var newAttributes = new SoulsAttributeSet(physicalPower: 100);

            await Arrange(dbState, isReadOnlyDatabase: true)
                .Handle(new UpdateItemAttributeSetCommand(item.Id, newAttributes))
                .AssertDatabase(dbState)
                .AssertPublishedEvent(new FailedToUpdateItemAttributeSetEvent(item.Id, newAttributes, ResultStatus.CriticalError, ["cannot write to readonly database"]));
        }

        [Fact]
        public async Task UpdateItemAttributeSet_WhenDoesNotExist_PublishesFailure()
        {
            var item = Entities.CreateItem();
            var dbState = DatabaseState.Empty;
            var newAttributes = new SoulsAttributeSet(physicalPower: 100);

            await Arrange(dbState)
                .Handle(new UpdateItemAttributeSetCommand(item.Id, newAttributes))
                .AssertDatabase(dbState)
                .AssertPublishedEvent(new FailedToUpdateItemAttributeSetEvent(item.Id, newAttributes, ResultStatus.NotFound, [$"no saved entity with id: '{item.Id}'"]));
        }

        private static HandlerTestSetup<UpdateItemAttributeSetCommandHandler> Arrange(
                DatabaseState? databaseState = null,
                bool isReadOnlyDatabase = false)
        {
            return new HandlerTestSetup<UpdateItemAttributeSetCommandHandler>(
                databaseState ?? DatabaseState.Empty,
                isReadOnlyDatabase,
                ConfigureMocker);
        }

        private static void ConfigureMocker(AutoMocker mocker)
        {
            mocker.Use<IRepository<Item>>(new FakeRepository<Item>());
        }
    }
}
