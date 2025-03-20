using Common.Infrastructure.Persistence;
using Common.LanguageExtensions.Utilities;
using MassTransit;
using Monsters.Core.Boundary.Events.Monsters;
using Presentation.Core.DataModels;

namespace Presentation.Core.EventHandlers.Monsters
{
    public class SyncElasticsearchWhenMonsterAddedEventHandler : IConsumer<MonsterAddedEvent>
    {
        private readonly IRepository<MonsterDetail> repository;

        public SyncElasticsearchWhenMonsterAddedEventHandler(IRepository<MonsterDetail> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<MonsterAddedEvent> context)
        {
            var monster = new MonsterDetail(
                id: context.Message.MonsterId,
                monsterName: context.Message.MonsterName,
                monsterLevel: context.Message.MonsterLevel,
                attributeSet: context.Message.AttributeSet);

            await repository.Create(monster, context.CancellationToken)
                .TapError(error => throw new Exception(string.Concat(error.Errors, ",")));
        }
    }
}
