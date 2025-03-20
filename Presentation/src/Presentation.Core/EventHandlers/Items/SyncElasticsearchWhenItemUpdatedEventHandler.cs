using Common.Infrastructure.Persistence;
using Common.LanguageExtensions.Utilities;
using MassTransit;
using Monsters.Core.Boundary.Events.Items;
using Presentation.Core.DataModels;

namespace Presentation.Core.EventHandlers.Items
{
    public class SyncElasticsearchWhenItemUpdatedEventHandler :
        IConsumer<ItemUpdatedEvent>,
        IConsumer<ItemNameUpdatedEvent>,
        IConsumer<ItemAttributeSetUpdatedEvent>
    {
        private readonly IRepository<ItemDetail> repository;

        public SyncElasticsearchWhenItemUpdatedEventHandler(IRepository<ItemDetail> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<ItemAttributeSetUpdatedEvent> context)
        {
            await repository.LoadById(context.Message.ItemId, context.CancellationToken)
                .Bind(item =>
                {
                    item.AttributeSet = context.Message.AttributeSet;

                    return repository.Update(item, context.CancellationToken);
                })
                .TapError(error => throw new Exception(string.Concat(error.Errors, ",")));
        }

        public async Task Consume(ConsumeContext<ItemNameUpdatedEvent> context)
        {
            await repository.LoadById(context.Message.ItemId, context.CancellationToken)
                .Bind(item =>
                {
                    item.ItemName = context.Message.ItemName;

                    return repository.Update(item, context.CancellationToken);
                })
                .TapError(error => throw new Exception(string.Concat(error.Errors, ",")));
        }

        public async Task Consume(ConsumeContext<ItemUpdatedEvent> context)
        {
            await repository.LoadById(context.Message.ItemId, context.CancellationToken)
                .Bind(item =>
                {
                    item.AttributeSet = context.Message.AttributeSet;
                    item.ItemName = context.Message.ItemName;

                    return repository.Update(item, context.CancellationToken);
                })
                .TapError(error => throw new Exception(string.Concat(error.Errors, ",")));
        }
    }
}
