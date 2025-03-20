using Common.Infrastructure.Persistence;
using Common.LanguageExtensions.Utilities;
using MassTransit;
using Monsters.Core.Boundary.Events.DropTables;
using Presentation.Core.DataModels;

namespace Presentation.Core.EventHandlers.DropTables
{
    public class SyncElasticsearchWhenDropTableRemovedEventHandler : IConsumer<DropTableRemovedEvent>
    {
        private readonly IRepository<DropTableDetail> repository;

        public SyncElasticsearchWhenDropTableRemovedEventHandler(IRepository<DropTableDetail> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<DropTableRemovedEvent> context)
        {
            await repository.LoadById(context.Message.TableId, context.CancellationToken)
                .Bind(table => repository.Delete(table, context.CancellationToken))
                .TapError(error => throw new Exception(string.Concat(error.Errors, ",")));
        }
    }
}
