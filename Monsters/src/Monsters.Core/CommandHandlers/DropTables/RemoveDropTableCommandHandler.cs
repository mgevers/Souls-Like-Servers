using Common.Infrastructure.Persistence;
using Common.LanguageExtensions.Utilities;
using MassTransit;
using Monsters.Core.Boundary.Events.DropTables;
using Monsters.Core.Commands.DropTables;
using Monsters.Core.Domain;

namespace Monsters.Core.CommandHandlers.DropTables
{
    public class RemoveDropTableCommandHandler : IConsumer<RemoveDropTableCommand>
    {
        private readonly IRepository<DropTable> repository;

        public RemoveDropTableCommandHandler(IRepository<DropTable> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<RemoveDropTableCommand> context)
        {
            await repository.LoadById(context.Message.TableId, context.CancellationToken)
                .Bind(dropTable => repository.Delete(dropTable, context.CancellationToken))
                .Tap(async () =>
                {
                    await context.Publish(
                        new DropTableRemovedEvent(context.Message.TableId),
                        context.CancellationToken);
                })
                .TapError(async error =>
                {
                    await context.Publish(
                        new FailedToRemoveDropTableEvent(
                            tableId: context.Message.TableId,
                            error.Status,
                            [.. error.Errors]),
                        context.CancellationToken);
                });
        }
    }
}
