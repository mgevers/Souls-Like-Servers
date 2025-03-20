using Common.Infrastructure.Persistence;
using Common.LanguageExtensions.Utilities;
using MassTransit;
using Monsters.Core.Boundary.Events.Items;
using Monsters.Core.Commands.Items;
using Monsters.Core.Domain;

namespace Monsters.Core.CommandHandlers.Items
{
    public class AddItemCommandHandler : IConsumer<AddItemCommand>
    {
        private readonly IRepository<Item> repository;

        public AddItemCommandHandler(IRepository<Item> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<AddItemCommand> context)
        {
            var entity = new Item(context.Message.ItemId, context.Message.ItemName, context.Message.AttributeSet);
            
            var result = await repository.Create(entity, context.CancellationToken)
                .Tap(async item =>
                {
                    await context.Publish(
                        new ItemAddedEvent(item.Id, item.Name, item.AttributeSet),
                        context.CancellationToken);
                })
                .TapError(async error =>
                {
                    await context.Publish(
                        new FailedToAddItemEvent(
                            context.Message.ItemId,
                            error.Status,
                            [.. error.Errors]),
                        context.CancellationToken);
                });

            await context.RespondAsync(result);
        }
    }
}
