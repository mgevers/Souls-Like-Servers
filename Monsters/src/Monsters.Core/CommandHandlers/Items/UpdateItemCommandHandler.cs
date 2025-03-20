using Common.Infrastructure.Persistence;
using Common.LanguageExtensions.Utilities;
using MassTransit;
using Monsters.Core.Boundary.Events.Items;
using Monsters.Core.Commands.Items;
using Monsters.Core.Domain;

namespace Monsters.Core.CommandHandlers.Items
{
    public class UpdateItemCommandHandler : IConsumer<UpdateItemCommand>
    {
        private readonly IRepository<Item> repository;

        public UpdateItemCommandHandler(IRepository<Item> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<UpdateItemCommand> context)
        {
            var result = await repository.LoadById(context.Message.ItemId, context.CancellationToken)
                .Bind(item =>
                {
                    item.Name = context.Message.ItemName;
                    item.AttributeSet = context.Message.AttributeSet;

                    return repository.Update(item, context.CancellationToken);
                })
                .Tap(async item =>
                {
                    await context.Publish(
                        new ItemUpdatedEvent(item.Id, item.Name, item.AttributeSet),
                        context.CancellationToken);
                })
                .TapError(async error =>
                {
                    await context.Publish(
                        new FailedToUpdateItemEvent(
                            context.Message.ItemId,
                            context.Message.ItemName,
                            context.Message.AttributeSet,
                            error.Status,
                            [.. error.Errors]),
                        context.CancellationToken);
                });

            await context.RespondAsync(result);
        }
    }
}
