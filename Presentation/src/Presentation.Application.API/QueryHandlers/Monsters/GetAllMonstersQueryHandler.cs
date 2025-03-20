using Ardalis.Result;
using Common.Infrastructure.Persistence;
using MediatR;
using Presentation.Core.DataModels;

namespace Presentation.Application.API.QueryHandlers.Monsters
{
    public record GetAllMonstersQuery : IRequest<Result<IReadOnlyList<MonsterDetail>>> { }

    public class GetAllMonstersQueryHandler : IRequestHandler<GetAllMonstersQuery, Result<IReadOnlyList<MonsterDetail>>>
    {
        private readonly IRepository<MonsterDetail> repository;

        public GetAllMonstersQueryHandler(IRepository<MonsterDetail> repository)
        {
            this.repository = repository;
        }

        public async Task<Result<IReadOnlyList<MonsterDetail>>> Handle(GetAllMonstersQuery request, CancellationToken cancellationToken)
        {
            var monsters = await repository.LoadAll(cancellationToken: cancellationToken);

            return monsters;
        }
    }
}
