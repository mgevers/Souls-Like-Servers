using Common.Infrastructure.Persistence;
using Common.LanguageExtensions.Utilities;
using MassTransit;
using Monsters.Core.Boundary.Events.Monsters;
using Monsters.Core.Commands.Monsters;
using Monsters.Core.Domain;

namespace Monsters.Core.CommandHandlers.Monsters
{
    public class AddMonsterCommandHandler : IConsumer<AddMonsterCommand>
    {
        private readonly IRepository<Monster> repository;

        public AddMonsterCommandHandler(IRepository<Monster> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<AddMonsterCommand> context)
        {
            var entity = new Monster(
                context.Message.MonsterId,
                context.Message.MonsterName,
                context.Message.MonsterLevel,
                context.Message.AttributeSet);

            var result = await repository.Create(entity, context.CancellationToken)
                .Tap(async monster =>
                {
                    await context.Publish(
                        new MonsterAddedEvent(
                            monster.Id,
                            monster.Name,
                            monster.Level,
                            monster.AttributeSet),
                        context.CancellationToken);
                })
                .TapError(async error =>
                {
                    await context.Publish(
                        new FailedToAddMonsterEvent(
                            context.Message.MonsterId,
                            error.Status,
                            [.. error.Errors],
                            context.Message.ConnectionId),
                        context.CancellationToken);
                });

            await context.RespondAsync(result);
        }
    }
}
