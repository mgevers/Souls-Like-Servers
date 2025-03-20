using Common.Infrastructure.Persistence;
using Common.LanguageExtensions.Utilities;
using MassTransit;
using Monsters.Core.Boundary.Events.Monsters;
using Monsters.Core.Commands.Monsters;
using Monsters.Core.Domain;

namespace Monsters.Core.CommandHandlers.Monsters
{
    public class UpdateMonsterCommandHandler : IConsumer<UpdateMonsterCommand>
    {
        private readonly IRepository<Monster> repository;

        public UpdateMonsterCommandHandler(IRepository<Monster> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<UpdateMonsterCommand> context)
        {
            var result = await repository.LoadById(context.Message.MonsterId, context.CancellationToken)
                .Bind(monster =>
                {
                    monster.Name = context.Message.MonsterName;
                    monster.Level = context.Message.MonsterLevel;
                    monster.AttributeSet = context.Message.AttributeSet;

                    return repository.Update(monster, context.CancellationToken);
                })
                .Tap(async monster =>
                {
                    await context.Publish(
                        new MonsterUpdatedEvent(
                            monster.Id,
                            monster.Name,
                            monster.Level,
                            monster.AttributeSet),
                        context.CancellationToken);
                })
                .TapError(async error =>
                {
                    await context.Publish(
                        new FailedToUpdateMonsterEvent(
                            monsterId: context.Message.MonsterId,
                            monsterName: context.Message.MonsterName,
                            monsterLevel: context.Message.MonsterLevel,
                            attributeSet: context.Message.AttributeSet,
                            status: error.Status,
                            errors: [.. error.Errors]),
                        context.CancellationToken);
                });

            await context.RespondAsync(result);
        }
    }
}
