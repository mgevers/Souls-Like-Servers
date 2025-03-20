using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Monsters.Core.Boundary.Events.Monsters;
using Presentation.Application.API.Hubs;
using Presentation.Core.Boundary.ValueObjects;

namespace Presentation.Application.API.EventHandlers.Monsters
{
    public class NotifyClientWhenMonstersChangedEventHandler :
        IConsumer<MonsterAddedEvent>,
        IConsumer<MonsterRemovedEvent>,
        IConsumer<MonsterUpdatedEvent>,
        IConsumer<MonsterAttributeSetUpdatedEvent>,
        IConsumer<MonsterNameUpdatedEvent>,
        IConsumer<MonsterLevelUpdatedEvent>,
        IConsumer<FailedToAddMonsterEvent>,
        IConsumer<FailedToRemoveMonsterEvent>,
        IConsumer<FailedToUpdateMonsterEvent>,
        IConsumer<FailedToUpdateMonsterAttributeSetEvent>,
        IConsumer<FailedToUpdateMonsterNameEvent>,
        IConsumer<FailedToUpdateMonsterLevelEvent>
    {
        private readonly IHubContext<PresentationHub, IPresentationHubClient> hubContext;

        public NotifyClientWhenMonstersChangedEventHandler(IHubContext<PresentationHub, IPresentationHubClient> hubContext)
        {
            this.hubContext = hubContext;
        }

        public Task Consume(ConsumeContext<MonsterAddedEvent> context)
        {
            var message = context.Message;
            var envelope = new Envelope<MonsterAddedEvent>(message);

            return hubContext.Clients.All.PushEnvelope(envelope);
        }

        public Task Consume(ConsumeContext<MonsterRemovedEvent> context)
        {
            var message = context.Message;
            var envelope = new Envelope<MonsterRemovedEvent>(message);

            return hubContext.Clients.All.PushEnvelope(envelope);
        }

        public Task Consume(ConsumeContext<MonsterUpdatedEvent> context)
        {
            var message = context.Message;
            var envelope = new Envelope<MonsterUpdatedEvent>(message);

            return hubContext.Clients.All.PushEnvelope(envelope);
        }

        public Task Consume(ConsumeContext<MonsterAttributeSetUpdatedEvent> context)
        {
            var message = context.Message;
            var envelope = new Envelope<MonsterAttributeSetUpdatedEvent>(message);

            return hubContext.Clients.All.PushEnvelope(envelope);
        }

        public Task Consume(ConsumeContext<MonsterNameUpdatedEvent> context)
        {
            var message = context.Message;
            var envelope = new Envelope<MonsterNameUpdatedEvent>(message);

            return hubContext.Clients.All.PushEnvelope(envelope);
        }

        public Task Consume(ConsumeContext<MonsterLevelUpdatedEvent> context)
        {
            var message = context.Message;
            var envelope = new Envelope<MonsterLevelUpdatedEvent>(message);

            return hubContext.Clients.All.PushEnvelope(envelope);
        }

        public Task Consume(ConsumeContext<FailedToAddMonsterEvent> context)
        {
            var message = context.Message;
            var envelope = new Envelope<FailedToAddMonsterEvent>(message);

            if (context.Message.ConnectionId == null)
            {
                return Task.CompletedTask;
            }

            return hubContext.Clients.Client(context.Message.ConnectionId).PushEnvelope(envelope);
        }

        public Task Consume(ConsumeContext<FailedToRemoveMonsterEvent> context)
        {
            var message = context.Message;
            var envelope = new Envelope<FailedToRemoveMonsterEvent>(message);

            return hubContext.Clients.All.PushEnvelope(envelope);
        }

        public Task Consume(ConsumeContext<FailedToUpdateMonsterEvent> context)
        {
            var message = context.Message;
            var envelope = new Envelope<FailedToUpdateMonsterEvent>(message);

            return hubContext.Clients.All.PushEnvelope(envelope);
        }

        public Task Consume(ConsumeContext<FailedToUpdateMonsterAttributeSetEvent> context)
        {
            var message = context.Message;
            var envelope = new Envelope<FailedToUpdateMonsterAttributeSetEvent>(message);

            return hubContext.Clients.All.PushEnvelope(envelope);
        }

        public Task Consume(ConsumeContext<FailedToUpdateMonsterNameEvent> context)
        {
            var message = context.Message;
            var envelope = new Envelope<FailedToUpdateMonsterNameEvent>(message);

            return hubContext.Clients.All.PushEnvelope(envelope);
        }

        public Task Consume(ConsumeContext<FailedToUpdateMonsterLevelEvent> context)
        {
            var message = context.Message;
            var envelope = new Envelope<FailedToUpdateMonsterLevelEvent>(message);

            return hubContext.Clients.All.PushEnvelope(envelope);
        }
    }
}
