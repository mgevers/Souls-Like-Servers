using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Monsters.Core.Commands.Monsters;
using Presentation.Application.API.Requests;

namespace Presentation.Application.API.Hubs
{
    public class PresentationHub : Hub<IPresentationHubClient>
    {
        private readonly IPublishEndpoint publishEndpoint;

        public PresentationHub(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        public Task AddMonster(AddMonsterRequest request)
        {
            var command = AddMonsterCommand.Create(
                request.MonsterId,
                request.MonsterName,
                request.MonsterLevel,
                request.AttributeSet,
                Context.ConnectionId);

            if (command.IsFailure)
            {
                throw new InvalidOperationException(command.Error);
            }

            return this.publishEndpoint.Publish(command.Value);
        }
    }
}
