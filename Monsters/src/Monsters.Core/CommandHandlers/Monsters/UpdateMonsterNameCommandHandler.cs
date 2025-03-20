using Common.Infrastructure.Persistence;
using Common.LanguageExtensions.Utilities;
using MassTransit;
using Monsters.Core.Boundary.Events.Monsters;
using Monsters.Core.Commands.Monsters;
using Monsters.Core.Domain;

namespace Monsters.Core.CommandHandlers.Monsters
{
    public class UpdateMonsterNameCommandHandler : IConsumer<UpdateMonsterNameCommand>
    {
        private readonly IRepository<Monster> repository;

        public UpdateMonsterNameCommandHandler(IRepository<Monster> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<UpdateMonsterNameCommand> context)
        {
            await repository.LoadById(context.Message.MonsterId, context.CancellationToken)
                .Bind(monster =>
                {
                    monster.Name = context.Message.MonsterName;

                    return repository.Update(monster, context.CancellationToken);
                })
                .Tap(async monster =>
                {
                    await context.Publish(
                        new MonsterNameUpdatedEvent(
                            monster.Id,
                            monster.Name),
                        context.CancellationToken);
                })
                .TapError(async error =>
                {
                    await context.Publish(
                        new FailedToUpdateMonsterNameEvent(
                            context.Message.MonsterId,
                            context.Message.MonsterName,
                            error.Status,
                            [.. error.Errors]),
                        context.CancellationToken);
                });
        }
    }
}
