using Common.Infrastructure.Persistence;
using Common.LanguageExtensions.Utilities;
using MassTransit;
using Monsters.Core.Boundary.Events.DropTables;
using Monsters.Core.Commands.DropTables;
using Monsters.Core.Domain;

namespace Monsters.Core.CommandHandlers.DropTables
{
    public class UpdateDropTableRollCountCommandHandler : IConsumer<UpdateDropTableRollCountCommand>
    {
        private readonly IRepository<DropTable> repository;

        public UpdateDropTableRollCountCommandHandler(IRepository<DropTable> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<UpdateDropTableRollCountCommand> context)
        {
            await repository.LoadById(context.Message.TableId, context.CancellationToken)
                .Bind(dropTable =>
                {
                    dropTable.RollCount = context.Message.RollCount;

                    return repository.Update(dropTable, context.CancellationToken);
                })
                .Tap(async dropTable =>
                {
                    await context.Publish(
                        new DropTableRollCountUpdatedEvent(
                            dropTable.Id,
                            dropTable.RollCount),
                        context.CancellationToken);
                })
                .TapError(async error =>
                {
                    await context.Publish(
                        new FailedToUpdateDropTableRollCountEvent(
                            context.Message.TableId,
                            context.Message.RollCount,
                            error.Status,
                            [.. error.Errors]),
                        context.CancellationToken);
                });
        }
    }
}
