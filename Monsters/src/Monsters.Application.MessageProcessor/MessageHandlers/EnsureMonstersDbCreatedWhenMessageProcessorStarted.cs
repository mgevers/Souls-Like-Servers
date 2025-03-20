using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Monsters.Application.MessageProcessor.Messages;
using Monsters.Persistence.SqlDatabase;

namespace Monsters.Application.MessageProcessor.MessageHandlers
{
    public class EnsureMonstersDbCreatedWhenMessageProcessorStarted : IConsumer<MonstersMessageProcessorStarted>
    {
        private readonly ILogger<EnsureMonstersDbCreatedWhenMessageProcessorStarted> logger;
        private readonly IDbContextFactory<MonstersDbContext> dbContextFactory;

        public EnsureMonstersDbCreatedWhenMessageProcessorStarted(
            ILogger<EnsureMonstersDbCreatedWhenMessageProcessorStarted> logger,
            IDbContextFactory<MonstersDbContext> dbContextFactory)
        {
            this.logger = logger;
            this.dbContextFactory = dbContextFactory;
        }

        public async Task Consume(ConsumeContext<MonstersMessageProcessorStarted> context)
        {
            logger.LogInformation("ensuring monsters database created");
            var dbContext = await dbContextFactory.CreateDbContextAsync(context.CancellationToken);

            await dbContext.Database.EnsureCreatedAsync(context.CancellationToken);
            await dbContext.Database.MigrateAsync(context.CancellationToken);
        }
    }
}
