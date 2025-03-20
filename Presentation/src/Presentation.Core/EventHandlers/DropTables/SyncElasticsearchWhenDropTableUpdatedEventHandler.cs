using Common.Infrastructure.Persistence;
using Common.LanguageExtensions.Utilities;
using MassTransit;
using Monsters.Core.Boundary.Events.DropTables;
using Presentation.Core.DataModels;

namespace Presentation.Core.EventHandlers.DropTables
{
    public class SyncElasticsearchWhenDropTableUpdatedEventHandler :
        IConsumer<RowAddedToDropTableEvent>,
        IConsumer<RowRemovedFromDropTableEvent>,
        IConsumer<DropTableRollCountUpdatedEvent>
    {
        private readonly IRepository<DropTableDetail> dropTableRepository;
        private readonly IRepository<ItemDetail> itemRepository;

        public SyncElasticsearchWhenDropTableUpdatedEventHandler(
            IRepository<DropTableDetail> dropTableRepository,
            IRepository<ItemDetail> itemRepository)
        {
            this.dropTableRepository = dropTableRepository;
            this.itemRepository = itemRepository;
        }

        public async Task Consume(ConsumeContext<RowAddedToDropTableEvent> context)
        {
            await dropTableRepository.LoadById(context.Message.TableId, context.CancellationToken)
                .Combine(table => itemRepository.LoadById(context.Message.Entry.Value.ItemId, context.CancellationToken))
                .Map(async kvp =>
                {
                    var table = kvp.Key;
                    var newRow = new DropRateDetail(
                        id: context.Message.Entry.Key,
                        item: kvp.Value,
                        dropRate: 1 / (double)context.Message.Entry.Value.DropRateDenominator);

                    table.Rows = [.. table.Rows, newRow];

                    return await dropTableRepository.Update(table, context.CancellationToken);
                })
                .TapError(error => throw new Exception(string.Concat(error.Errors, ",")));
        }

        public async Task Consume(ConsumeContext<RowRemovedFromDropTableEvent> context)
        {
            await dropTableRepository.LoadById(context.Message.TableId, context.CancellationToken)
                .Bind(async table =>
                {
                    if (!table.Rows.Any(row => row.Id == context.Message.RowId))
                    {
                        throw new Exception($"row with id {context.Message.RowId} not found");
                    }

                    table.Rows = [.. table.Rows.Where(row => row.Id != context.Message.RowId)];

                    return await dropTableRepository.Update(table, context.CancellationToken);
                })
                .TapError(error => throw new Exception(string.Concat(error.Errors, ",")));
        }

        public async Task Consume(ConsumeContext<DropTableRollCountUpdatedEvent> context)
        {
            await dropTableRepository.LoadById(context.Message.TableId, context.CancellationToken)
                .Bind(async table =>
                {
                    table.RollCount = context.Message.RollCount;

                    return await dropTableRepository.Update(table, context.CancellationToken);
                })
                .TapError(error => throw new Exception(string.Concat(error.Errors, ",")));
        }
    }
}
