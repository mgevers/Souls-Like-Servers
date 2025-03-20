using Common.Infrastructure.Persistence;
using Common.LanguageExtensions.Utilities;
using MassTransit;
using Monsters.Core.Boundary.Events.Items;
using Monsters.Core.Commands.Items;
using Monsters.Core.Domain;

namespace Monsters.Core.CommandHandlers.Items
{
    public class UpdateItemNameCommandHandler : IConsumer<UpdateItemNameCommand>
    {
        private readonly IRepository<Item> repository;

        public UpdateItemNameCommandHandler(IRepository<Item> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<UpdateItemNameCommand> context)
        {
            await repository.LoadById(context.Message.ItemId, context.CancellationToken)
                .Bind(item =>
                {
                    item.Name = context.Message.ItemName;

                    return repository.Update(item, context.CancellationToken);
                })
                .Tap(async item =>
                {
                    await context.Publish(
                        new ItemNameUpdatedEvent(item.Id, item.Name),
                        context.CancellationToken);
                })
                .TapError(async error =>
                {
                    await context.Publish(
                        new FailedToUpdateItemNameEvent(
                            context.Message.ItemId,
                            context.Message.ItemName,
                            error.Status,
                            [.. error.Errors]),
                        context.CancellationToken);
                });
        }
    }
}
