using Common.Infrastructure.Persistence;
using Common.LanguageExtensions.Utilities;
using MassTransit;
using Monsters.Core.Boundary.Events.Items;
using Monsters.Core.Commands.Items;
using Monsters.Core.Domain;

namespace Monsters.Core.CommandHandlers.Items
{
    public class RemoveItemCommandHandler : IConsumer<RemoveItemCommand>
    {
        private readonly IRepository<Item> repository;

        public RemoveItemCommandHandler(IRepository<Item> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<RemoveItemCommand> context)
        {
            var result = await repository.LoadById(context.Message.ItemId, context.CancellationToken)
                .Bind(item => repository.Delete(item, context.CancellationToken))
                .Tap(async () =>
                {
                    await context.Publish(
                        new ItemRemovedEvent(context.Message.ItemId),
                        context.CancellationToken);
                })
                .TapError(async error =>
                {
                    await context.Publish(
                        new FailedToRemoveItemEvent(
                            context.Message.ItemId,
                            error.Status,
                            [.. error.Errors]),
                        context.CancellationToken);
                });

            await context.RespondAsync(result);
        }
    }
}
