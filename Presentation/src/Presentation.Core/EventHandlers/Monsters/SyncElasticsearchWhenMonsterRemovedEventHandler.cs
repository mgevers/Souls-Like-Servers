using Common.Infrastructure.Persistence;
using Common.LanguageExtensions.Utilities;
using MassTransit;
using Monsters.Core.Boundary.Events.Monsters;
using Presentation.Core.DataModels;

namespace Presentation.Core.EventHandlers.Monsters
{
    public class SyncElasticsearchWhenMonsterRemovedEventHandler : IConsumer<MonsterRemovedEvent>
    {
        private readonly IRepository<MonsterDetail> repository;

        public SyncElasticsearchWhenMonsterRemovedEventHandler(IRepository<MonsterDetail> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<MonsterRemovedEvent> context)
        {
            await repository.LoadById(context.Message.MonsterId, context.CancellationToken)
                .Bind(item => repository.Delete(item, context.CancellationToken))
                .TapError(error => throw new Exception(string.Concat(error.Errors, ",")));
        }
    }
}
