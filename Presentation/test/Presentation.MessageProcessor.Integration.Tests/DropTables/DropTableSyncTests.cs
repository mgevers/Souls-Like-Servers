using Common.Infrastructure.Persistence;
using Common.Testing.Assert;
using Common.Testing.Integration.Containers;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Monsters.Core.Boundary.Events.DropTables;
using Monsters.Core.Boundary.Events.Items;
using Monsters.Core.Boundary.Events.Monsters;
using Monsters.Core.Boundary.ValueObjects;
using Presentatin.Application.MessageProcessor.Messages;
using Presentation.Core.DataModels;
using Presentation.Testing;

namespace Presentation.MessageProcessor.Integration.Tests.DropTables
{
    public class DropTableSyncTests :
        IClassFixture<PresentationMessageProcessorApplicationFactory>,
        IClassFixture<ElasticsearchContainer>
    {
        private readonly PresentationMessageProcessorApplicationFactory appFactory;

        public DropTableSyncTests(
            PresentationMessageProcessorApplicationFactory appFactory,
            ElasticsearchContainer elasticsearchContainer)
        {
            this.appFactory = appFactory;
            this.appFactory.Server.PreserveExecutionContext = true;

            this.appFactory.ElasticsearchContainer = elasticsearchContainer;
        }

        [Fact]
        public async Task SyncronizesDropTableData()
        {
            var tableId = Guid.Parse("44479c86-c861-4b75-98be-bc23d0e87f6b");
            var itemId = Guid.Parse("27927854-7bd4-4dbe-9a94-b5a817617cfb");
            var monsterId = Guid.Parse("1650c2a9-9aee-4b93-926e-d6250cbede69");
            var rowId = Guid.Parse("e1c977f3-0d3a-4e9b-90d0-a33e5dc8d083");

            var testHarness = appFactory.Services.GetRequiredService<ITestHarness>();
            await testHarness.Start();

            var monster = DataModelDetails.CreateMonster(id: monsterId);
            var item = DataModelDetails.CreateItem(id: itemId);
            var row = DataModelDetails.CreateDropRateDetail(id: rowId, item: item, dropRate: .5);
            var table = DataModelDetails.CreateDropTable(id: tableId, monster: monster, rows: [row]);

            try
            {
                await testHarness.Consumed.Any<PresentationMessageProcessorStarted>();

                await Task.Delay(1000);

                await testHarness.Bus.Publish(new ItemAddedEvent(item.Id, new ItemName(item.ItemName), item.AttributeSet));
                await testHarness.Consumed.Any<ItemAddedEvent>();

                await testHarness.Bus.Publish(new MonsterAddedEvent(
                    monster.Id,
                    new MonsterName(monster.MonsterName),
                    new MonsterLevel(monster.MonsterLevel),
                    monster.AttributeSet));
                await testHarness.Consumed.Any<MonsterAddedEvent>();

                await Task.Delay(1000);

                await testHarness.Bus.Publish(new DropTableAddedEvent(
                    tableId: table.Id,
                    monsterId: table.Monster.Id,
                    new RollCount(table.RollCount),
                    entries: [new KeyValuePair<Guid, DropTableEntry>(row.Id, new DropTableEntry(item.Id, new DropRateDenominator(2)))]));
                await testHarness.Consumed.Any<DropTableAddedEvent>();

                var itemRepository = appFactory.Services.GetRequiredService<IRepository<ItemDetail>>();
                var monsterRepository = appFactory.Services.GetRequiredService<IRepository<MonsterDetail>>();
                var tableRepository = appFactory.Services.GetRequiredService<IRepository<DropTableDetail>>();

                await Task.Delay(1000);

                var loadedItem = await itemRepository.LoadById(item.Id);
                var loadedMonster = await monsterRepository.LoadById(monster.Id);
                var loadedTable = await tableRepository.LoadById(table.Id);

                Assert.True(loadedItem.IsSuccess, string.Concat(loadedItem.Errors, ","));
                Assert.True(loadedMonster.IsSuccess, string.Concat(loadedMonster.Errors, ","));
                Assert.True(loadedTable.IsSuccess, string.Concat(loadedTable.Errors, ","));

                AssertExtensions.DeepEqual(item, loadedItem.Value);
                AssertExtensions.DeepEqual(monster, loadedMonster.Value);
                AssertExtensions.DeepEqual(table, loadedTable.Value);
            }
            finally
            {
                await testHarness.Stop();
            }
        }
    }
}
