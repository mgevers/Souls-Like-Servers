using Common.Infrastructure.Persistence;
using Common.LanguageExtensions.Utilities;
using MassTransit;
using Monsters.Core.Boundary.Events.Items;
using Presentation.Core.DataModels;

namespace Presentation.Core.EventHandlers.Items
{
    public class SyncElasticsearchWhenItemAddedEventHandler : IConsumer<ItemAddedEvent>
    {
        private readonly IRepository<ItemDetail> repository;

        public SyncElasticsearchWhenItemAddedEventHandler(IRepository<ItemDetail> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<ItemAddedEvent> context)
        {
            var item = new ItemDetail(
                id: context.Message.ItemId,
                itemName: context.Message.ItemName,
                attributeSet: context.Message.AttributeSet);

            await repository.Create(item, context.CancellationToken)
                .TapError(error => throw new Exception(string.Concat(error.Errors, ",")));
        }
    }
}
