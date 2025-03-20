using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Monsters.Core.Boundary.Events.DropTables;
using Monsters.Core.Boundary.Events.Items;
using Monsters.Core.Boundary.Events.Monsters;
using Monsters.Core.Boundary.ValueObjects;
using Monsters.Core.Commands.DropTables;
using Monsters.Core.Commands.Items;
using Monsters.Core.Commands.Monsters;
using Monsters.Persistence.Repositories;
using Monsters.Persistence.SqlDatabase;
using Monsters.Testing;

namespace Monsters.Integration.Tests.DropTables
{
    public class DropTableTests : IClassFixture<MonstersMessageProcessorApplicationFactory>
    {
        private readonly MonstersMessageProcessorApplicationFactory appFactory;

        public DropTableTests(MonstersMessageProcessorApplicationFactory appFactory)
        {
            this.appFactory = appFactory;
        }

        [Fact]
        public async Task CanCreateDropTable()
        {
            var testHarnes = appFactory.Services.GetRequiredService<ITestHarness>();
            await testHarnes.Start();

            try
            {
                var monster = Entities.CreateMonster();
                var item = Entities.CreateItem();
                var itemDropRow = Entities.CreateDropTableRow(item: item);
                var dropTable = Entities.CreateDropTable([itemDropRow], monster: monster);

                await testHarnes.Bus.Publish(new AddItemCommand(item.Id, item.Name, item.AttributeSet));
                await testHarnes.Published.Any<ItemAddedEvent>();

                await testHarnes.Bus.Publish(new AddMonsterCommand(monster.Id, monster.Name, monster.Level, monster.AttributeSet));
                await testHarnes.Published.Any<MonsterAddedEvent>();

                await testHarnes.Bus.Publish(new AddDropTableCommand(
                    dropTable.Id,
                    monster.Id,
                    dropTable.RollCount,
                    [.. dropTable.Rows.Select(row => new DropTableEntry(row.Item.Id, row.DropRateDenominator))]));
                await testHarnes.Published.Any<DropTableAddedEvent>();

                using (var scope = appFactory.Services.CreateScope())
                {
                    var dbContextFactory = appFactory.Services.GetRequiredService<IDbContextFactory<MonstersDbContext>>();
                    var dbContext = dbContextFactory.CreateDbContext();
                    var repository = new DropTableRepository(dbContext);

                    var loadedTable = await repository.LoadById(dropTable.Id);

                    Assert.True(loadedTable.IsSuccess, string.Concat(loadedTable.Errors, ","));
                    var loadedRows = loadedTable.Value.Rows;
                    Assert.NotEmpty(loadedRows);
                }
            }
            finally
            {
                await testHarnes.Stop();
            }
        }
    }
}
