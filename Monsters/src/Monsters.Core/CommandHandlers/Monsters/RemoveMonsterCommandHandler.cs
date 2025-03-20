using Common.Infrastructure.Persistence;
using Common.LanguageExtensions.Utilities;
using MassTransit;
using Monsters.Core.Boundary.Events.Monsters;
using Monsters.Core.Commands.Monsters;
using Monsters.Core.Domain;

namespace Monsters.Core.CommandHandlers.Monsters
{
    public class RemoveMonsterCommandHandler : IConsumer<RemoveMonsterCommand>
    {
        private readonly IRepository<Monster> repository;

        public RemoveMonsterCommandHandler(IRepository<Monster> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<RemoveMonsterCommand> context)
        {
            var result = await repository.LoadById(context.Message.MonsterId, context.CancellationToken)
                .Bind(monster => repository.Delete(monster, context.CancellationToken))
                .Tap(async () =>
                {
                    await context.Publish(new MonsterRemovedEvent(context.Message.MonsterId), context.CancellationToken);
                })
                .TapError(async error =>
                {
                    await context.Publish(
                        new FailedToRemoveMonsterEvent(
                            context.Message.MonsterId,
                            error.Status,
                            [.. error.Errors]),
                        context.CancellationToken);
                });

            await context.RespondAsync(result);
        }
    }
}
