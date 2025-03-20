using Common.Infrastructure.Persistence;
using Common.LanguageExtensions.Utilities;
using MassTransit;
using Monsters.Core.Boundary.Events.Monsters;
using Presentation.Core.DataModels;

namespace Presentation.Core.EventHandlers.Monsters
{
    public class SyncElasticsearchWhenMonsterUpdatedEventHandler :
        IConsumer<MonsterUpdatedEvent>,
        IConsumer<MonsterNameUpdatedEvent>,
        IConsumer<MonsterLevelUpdatedEvent>,
        IConsumer<MonsterAttributeSetUpdatedEvent>
    {
        private readonly IRepository<MonsterDetail> repository;

        public SyncElasticsearchWhenMonsterUpdatedEventHandler(IRepository<MonsterDetail> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<MonsterAttributeSetUpdatedEvent> context)
        {
            await repository.LoadById(context.Message.MonsterId, context.CancellationToken)
                .Bind(monster =>
                {
                    monster.AttributeSet = context.Message.AttributeSet;

                    return repository.Update(monster, context.CancellationToken);
                })
                .TapError(error => throw new Exception(string.Concat(error.Errors, ",")));
        }

        public async Task Consume(ConsumeContext<MonsterNameUpdatedEvent> context)
        {
            await repository.LoadById(context.Message.MonsterId, context.CancellationToken)
                .Bind(monster =>
                {
                    monster.MonsterName = context.Message.MonsterName;

                    return repository.Update(monster, context.CancellationToken);
                })
                .TapError(error => throw new Exception(string.Concat(error.Errors, ",")));
        }

        public async Task Consume(ConsumeContext<MonsterLevelUpdatedEvent> context)
        {
            await repository.LoadById(context.Message.MonsterId, context.CancellationToken)
                .Bind(monster =>
                {
                    monster.MonsterLevel = context.Message.MonsterLevel;

                    return repository.Update(monster, context.CancellationToken);
                })
                .TapError(error => throw new Exception(string.Concat(error.Errors, ",")));
        }

        public async Task Consume(ConsumeContext<MonsterUpdatedEvent> context)
        {
            await repository.LoadById(context.Message.MonsterId, context.CancellationToken)
                .Bind(monster =>
                {
                    monster.AttributeSet = context.Message.AttributeSet;
                    monster.MonsterName = context.Message.MonsterName;
                    monster.MonsterLevel = context.Message.MonsterLevel;

                    return repository.Update(monster, context.CancellationToken);
                })
                .TapError(error => throw new Exception(string.Concat(error.Errors, ",")));
        }
    }
}
