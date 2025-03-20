using Ardalis.Result;
using Common.Infrastructure.Persistence;
using Common.LanguageExtensions.TestableAlternatives;
using Common.LanguageExtensions.Utilities;
using MassTransit;
using Monsters.Core.Boundary.Events.DropTables;
using Monsters.Core.Boundary.ValueObjects;
using Monsters.Core.Commands.DropTables;
using Monsters.Core.Domain;

namespace Monsters.Core.CommandHandlers.DropTables
{
    public class AddDropTableCommandHandler : IConsumer<AddDropTableCommand>
    {
        private readonly IRepository<DropTable> dropTableRepository;
        private readonly IRepository<Item> itemRepository;
        private readonly IRepository<Monster> monsterRepository;

        public AddDropTableCommandHandler(
            IRepository<DropTable> dropTableRepository,
            IRepository<Item> itemRepository,
            IRepository<Monster> monsterRepository)
        {
            this.dropTableRepository = dropTableRepository;
            this.itemRepository = itemRepository;
            this.monsterRepository = monsterRepository;
        }

        public async Task Consume(ConsumeContext<AddDropTableCommand> context)
        {
            await itemRepository.LoadByIds([.. context.Message.Entries.Select(entry => entry.ItemId)], context.CancellationToken)
                .Combine(items => monsterRepository.LoadById(context.Message.MonsterId, context.CancellationToken))
                .Map(kvp =>
                {
                    var itemDict = kvp.Key.ToDictionary(item => item.Id, item => item);

                    var rows = context.Message.Entries
                        .Select(entry => new DropTableRow(GuidProvider.NewGuid(), itemDict[entry.ItemId], entry.DropRateDenominator))
                        .ToList();

                    var dropTable = new DropTable(
                        id: context.Message.TableId,
                        monster: kvp.Value,
                        rollCount: context.Message.RollCount,
                        rows: rows);

                    return dropTableRepository.Create(dropTable, context.CancellationToken);
                })
                .Tap(async table =>
                {
                    var entries = table.Rows
                        .Select(row => new KeyValuePair<Guid, DropTableEntry>(row.Id, new DropTableEntry(row.Item.Id, row.DropRateDenominator)))
                        .ToList();

                    await context.Publish(
                        new DropTableAddedEvent(
                            tableId: table.Id,
                            monsterId: table.Monster.Id,
                            table.RollCount,
                            entries),
                        context.CancellationToken);
                })
                .TapError(async error =>
                {
                    await context.Publish(
                        new FailedToAddDropTableEvent(context.Message.TableId, error.Status, [.. error.Errors]),
                        context.CancellationToken);
                });
        }
    }
}
