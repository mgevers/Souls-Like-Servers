using Ardalis.Result;
using Common.Infrastructure.Persistence;
using Common.LanguageExtensions.Utilities;
using MassTransit;
using Monsters.Core.Boundary.Events.DropTables;
using Monsters.Core.Boundary.ValueObjects;
using Monsters.Core.Commands.DropTables;
using Monsters.Core.Domain;

namespace Monsters.Core.CommandHandlers.DropTables
{
    public class AddRowToDropTableCommandHandler : IConsumer<AddRowToDropTableCommand>
    {
        private readonly IRepository<DropTable> dropTableRepository;
        private readonly IRepository<Item> itemRepository;

        public AddRowToDropTableCommandHandler(
            IRepository<DropTable> dropTableRepository,
            IRepository<Item> itemRepository)
        {
            this.dropTableRepository = dropTableRepository;
            this.itemRepository = itemRepository;
        }

        public async Task Consume(ConsumeContext<AddRowToDropTableCommand> context)
        {
            await dropTableRepository.LoadById(context.Message.TableId, context.CancellationToken)
                .Combine(table => itemRepository.LoadById(context.Message.Entry.ItemId))
                .Map(kvp =>
                {
                    var dropTable = kvp.Key;
                    var row = new DropTableRow(context.Message.RowId, kvp.Value, context.Message.Entry.DropRateDenominator);

                    var result = dropTable.AddRow(row);
                    return result;
                })
                .Bind(table => dropTableRepository.Update(table, context.CancellationToken))
                .Tap(async kvp =>
                {
                    await context.Publish(
                        new RowAddedToDropTableEvent(
                            tableId: context.Message.TableId,
                            entry: new KeyValuePair<Guid, DropTableEntry>(context.Message.RowId, context.Message.Entry)),
                        context.CancellationToken);
                })
                .TapError(async error =>
                {
                    IReadOnlyCollection<string> errors = error.Errors
                        .Concat(error.ValidationErrors.Select(e => e.ErrorMessage))
                        .ToList();

                    await context.Publish(
                        new FailedToAddRowToDropTableEvent(
                            tableId: context.Message.TableId,
                            rowId: context.Message.RowId,
                            error.Status,
                            errors),
                        context.CancellationToken);
                });
        }
    }
}
