using Common.Infrastructure.Persistence;
using Common.LanguageExtensions.Utilities;
using MassTransit;
using Monsters.Core.Boundary.Events.Items;
using Presentation.Core.DataModels;

namespace Presentation.Core.EventHandlers.Items
{
    public class SyncElasticsearchWhenItemRemovedEventHandler : IConsumer<ItemRemovedEvent>
    {
        private readonly IRepository<ItemDetail> repository;

        public SyncElasticsearchWhenItemRemovedEventHandler(IRepository<ItemDetail> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<ItemRemovedEvent> context)
        {
            await repository.LoadById(context.Message.ItemId, context.CancellationToken)
                .Bind(item => repository.Delete(item, context.CancellationToken))
                .TapError(error => throw new Exception(string.Concat(error.Errors, ",")));
        }
    }
}
