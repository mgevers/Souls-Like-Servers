using Common.Infrastructure.Persistence;
using Common.LanguageExtensions.Utilities;
using MassTransit;
using Monsters.Core.Boundary.Events.Monsters;
using Monsters.Core.Commands.Monsters;
using Monsters.Core.Domain;

namespace Monsters.Core.CommandHandlers.Monsters
{
    public class UpdateMonsterLevelCommandHandler : IConsumer<UpdateMonsterLevelCommand>
    {
        private readonly IRepository<Monster> repository;

        public UpdateMonsterLevelCommandHandler(IRepository<Monster> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<UpdateMonsterLevelCommand> context)
        {
            await repository.LoadById(context.Message.MonsterId, context.CancellationToken)
                .Bind(monster =>
                {
                    monster.Level = context.Message.MonsterLevel;

                    return repository.Update(monster, context.CancellationToken);
                })
                .Tap(async monster =>
                {
                    await context.Publish(
                        new MonsterLevelUpdatedEvent(
                            monster.Id,
                            monster.Level),
                        context.CancellationToken);
                })
                .TapError(async error =>
                {
                    await context.Publish(
                        new FailedToUpdateMonsterLevelEvent(
                            context.Message.MonsterId,
                            context.Message.MonsterLevel,
                            error.Status,
                            [.. error.Errors]),
                        context.CancellationToken);
                });
        }
    }
}
