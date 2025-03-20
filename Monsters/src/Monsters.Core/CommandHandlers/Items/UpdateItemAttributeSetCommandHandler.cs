using Common.Infrastructure.Persistence;
using Common.LanguageExtensions.Utilities;
using MassTransit;
using Monsters.Core.Boundary.Events.Items;
using Monsters.Core.Commands.Items;
using Monsters.Core.Domain;

namespace Monsters.Core.CommandHandlers.Items
{
    public class UpdateItemAttributeSetCommandHandler : IConsumer<UpdateItemAttributeSetCommand>
    {
        private readonly IRepository<Item> repository;

        public UpdateItemAttributeSetCommandHandler(IRepository<Item> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<UpdateItemAttributeSetCommand> context)
        {
            await repository.LoadById(context.Message.ItemId, context.CancellationToken)
                .Bind(item =>
                {
                    item.AttributeSet = context.Message.AttributeSet;

                    return repository.Update(item, context.CancellationToken);
                })
                .Tap(async item =>
                {
                    await context.Publish(
                        new ItemAttributeSetUpdatedEvent(item.Id, item.AttributeSet),
                        context.CancellationToken);
                })
                .TapError(async error =>
                {
                    await context.Publish(
                        new FailedToUpdateItemAttributeSetEvent(
                            context.Message.ItemId,
                            context.Message.AttributeSet,
                            error.Status,
                            [.. error.Errors]),
                        context.CancellationToken);
                });
        }
    }
}
