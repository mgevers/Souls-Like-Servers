using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Monsters.Core.Commands.Monsters;
using Monsters.Core.Domain;
using Presentation.Application.API.QueryHandlers.Monsters;
using Presentation.Application.API.Requests;

namespace Presentation.Application.API.Controllers
{
    [ApiController]
    [Route("api/presentation/monsters")]
    public class MonstersController : ControllerBase
    {
        private readonly ILogger<MonstersController> logger;
        private readonly IMediator mediator;
        private readonly IRequestClient<AddMonsterCommand> addMonsterClient;
        private readonly IRequestClient<UpdateMonsterCommand> udpateMonsterClient;
        private readonly IRequestClient<RemoveMonsterCommand> removeMonsterClient;

        public MonstersController(
            ILogger<MonstersController> logger,
            IMediator mediator,
            IRequestClient<AddMonsterCommand> addMonsterClient,
            IRequestClient<UpdateMonsterCommand> udpateMonsterClient,
            IRequestClient<RemoveMonsterCommand> removeMonsterClient)
        {
            this.logger = logger;
            this.mediator = mediator;
            this.addMonsterClient = addMonsterClient;
            this.udpateMonsterClient = udpateMonsterClient;
            this.removeMonsterClient = removeMonsterClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMonsters(CancellationToken cancellation)
        {
            var result = await mediator.Send(new GetAllMonstersQuery(), cancellation);

            IConvertToActionResult actionResult = result.ToActionResult(this);
            return actionResult.Convert();
        }

        [HttpPost]
        public async Task<IActionResult> AddMonster(AddMonsterRequest request, CancellationToken cancellation)
        {
            var claims = this.User.Claims.ToList();

            var command = AddMonsterCommand.Create(
                request.MonsterId,
                request.MonsterName,
                request.MonsterLevel,
                request.AttributeSet);

            if (command.IsFailure)
            {
                return BadRequest(command.Error);
            }

            var response = await addMonsterClient.GetResponse<Result<Monster>>(command.Value, cancellation);
            IConvertToActionResult actionResult = response.Message.ToActionResult(this);
            return actionResult.Convert();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMonster(UpdateMonsterRequest request, CancellationToken cancellation)
        {
            var command = UpdateMonsterCommand.Create(
                request.MonsterId,
                request.MonsterName,
                request.MonsterLevel,
                request.AttributeSet);

            if (command.IsFailure)
            {
                return BadRequest(command.Error);
            }

            var response = await udpateMonsterClient.GetResponse<Result<Monster>>(command.Value, cancellation);
            IConvertToActionResult actionResult = response.Message.ToActionResult(this);
            return actionResult.Convert();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveMonster(Guid monsterId, CancellationToken cancellation)
        {
            var command = new RemoveMonsterCommand(monsterId);

            var response = await removeMonsterClient.GetResponse<Result<Monster>>(command, cancellation);
            IConvertToActionResult actionResult = response.Message.ToActionResult(this);
            return actionResult.Convert();
        }
    }
}
