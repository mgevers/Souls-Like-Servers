using Common.Infrastructure.Persistence;
using Common.LanguageExtensions.Utilities;
using MassTransit;
using Monsters.Core.Boundary.Events.DropTables;
using Monsters.Core.Commands.DropTables;
using Monsters.Core.Domain;

namespace Monsters.Core.CommandHandlers.DropTables
{
    public class RemoveRowFromDropTableCommandHandler : IConsumer<RemoveRowFromDropTableCommand>
    {
        private readonly IRepository<DropTable> repository;

        public RemoveRowFromDropTableCommandHandler(IRepository<DropTable> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<RemoveRowFromDropTableCommand> context)
        {
            await repository.LoadById(context.Message.TableId, context.CancellationToken)
                .Bind(dropTable =>
                {
                    dropTable.RemoveRow(context.Message.RowId);

                    return repository.Update(dropTable, context.CancellationToken);
                })
                .Tap(async dropTable =>
                {
                    await context.Publish(
                        new RowRemovedFromDropTableEvent(
                            tableId: dropTable.Id,
                            rowId: context.Message.RowId),
                        context.CancellationToken);
                })
                .TapError(async error =>
                {
                    await context.Publish(
                        new FailedToRemoveRowFromDropTableEvent(
                            tableId: context.Message.TableId,
                            rowId: context.Message.RowId,
                            error.Status,
                            [.. error.Errors]),
                        context.CancellationToken);
                });
        }
    }
}
