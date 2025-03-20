using Common.Infrastructure.Persistence;
using Common.LanguageExtensions.Utilities;
using MassTransit;
using Monsters.Core.Boundary.Events.Monsters;
using Monsters.Core.Commands.Monsters;
using Monsters.Core.Domain;

namespace Monsters.Core.CommandHandlers.Monsters
{
    public class UpdateMonsterAttributeSetCommandHandler : IConsumer<UpdateMonsterAttributeSetCommand>
    {
        private readonly IRepository<Monster> repository;

        public UpdateMonsterAttributeSetCommandHandler(IRepository<Monster> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<UpdateMonsterAttributeSetCommand> context)
        {
            await repository.LoadById(context.Message.MonsterId, context.CancellationToken)
                .Bind(monster =>
                {
                    monster.AttributeSet = context.Message.AttributeSet;

                    return repository.Update(monster, context.CancellationToken);
                })
                .Tap(async monster =>
                {
                    await context.Publish(
                        new MonsterAttributeSetUpdatedEvent(
                            monster.Id,
                            monster.AttributeSet),
                        context.CancellationToken);
                })
                .TapError(async error =>
                {
                    await context.Publish(
                        new FailedToUpdateMonsterAttributeSetEvent(
                            context.Message.MonsterId,
                            context.Message.AttributeSet,
                            error.Status,
                            [.. error.Errors]),
                        context.CancellationToken);
                });
        }
    }
}
