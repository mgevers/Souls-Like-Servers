using Common.Infrastructure.Persistence;
using Common.LanguageExtensions.Utilities;
using MassTransit;
using Monsters.Core.Boundary.Events.DropTables;
using Presentation.Core.DataModels;

namespace Presentation.Core.EventHandlers.DropTables
{
    public class SyncElasticsearchWhenDropTableAddedEventHandler : IConsumer<DropTableAddedEvent>
    {
        private readonly IRepository<MonsterDetail> monsterRepository;
        private readonly IRepository<DropTableDetail> dropTableRepository;
        private readonly IRepository<ItemDetail> itemRepository;

        public SyncElasticsearchWhenDropTableAddedEventHandler(
            IRepository<MonsterDetail> monsterRepository,
            IRepository<DropTableDetail> dropTableRepository,
            IRepository<ItemDetail> itemRepository)
        {
            this.monsterRepository = monsterRepository;
            this.dropTableRepository = dropTableRepository;
            this.itemRepository = itemRepository;
        }

        public async Task Consume(ConsumeContext<DropTableAddedEvent> context)
        {
            var itemIds = context.Message.Entries
                .Select(e => e.Value.ItemId)
                .ToList();

            await monsterRepository.LoadById(context.Message.MonsterId, context.CancellationToken)
                .Combine(monster => itemRepository.LoadByIds(itemIds, context.CancellationToken))
                .Map(async kvp =>
                {
                    var itemDict = kvp.Value
                        .ToDictionary(item => item.Id, item => item);

                    var rows = context.Message.Entries
                        .Select(entry => new DropRateDetail(
                            id: entry.Key,
                            item: itemDict[entry.Value.ItemId],
                            dropRate: 1 / (double)entry.Value.DropRateDenominator))
                        .ToList();

                    var dropTable = new DropTableDetail(
                        id: context.Message.TableId,
                        monster: kvp.Key,
                        rollCount: context.Message.RollCount,
                        rows: rows);

                    return await dropTableRepository.Create(dropTable, context.CancellationToken);
                })
                .TapError(error => throw new Exception(string.Concat(error.Errors, ",")));
        }
    }
}
