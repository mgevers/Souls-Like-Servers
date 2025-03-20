using Ardalis.Result;
using Common.Infrastructure.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Monsters.Core.Domain;
using Monsters.Persistence.SqlDatabase;

namespace Monsters.Persistence.Repositories
{
    public class DropTableRepository : EFRepository<DropTable, MonstersDbContext>
    {
        public DropTableRepository(MonstersDbContext dbContext)
            : base(dbContext)
        {
        }

        public async override Task<Result<IReadOnlyList<DropTable>>> LoadAll(int count = 1_000, CancellationToken cancellationToken = default)
        {
            var entities = await this.dbContext.DropTables
                .Include(table => table.Rows)
                .ToListAsync(cancellationToken);

            return entities == null
                ? Result<IReadOnlyList<DropTable>>.NotFound()
                : Result<IReadOnlyList<DropTable>>.Success(entities);
        }

        public async override Task<Result<DropTable>> LoadById(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await this.dbContext.DropTables
                .Include(table => table.Rows)
                .SingleOrDefaultAsync(p => id!.Equals(p.Id), cancellationToken);

            return entity == null
                ? Result<DropTable>.NotFound($"entity with id '{id}' not found")
                : Result<DropTable>.Success(entity);
        }

        public async override Task<Result<IReadOnlyList<DropTable>>> LoadByIds(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken = default)
        {
            var entities = await this.dbContext.DropTables
                .Include(table => table.Rows)
                .Where(table => ids.Contains(table.Id))
                .ToListAsync(cancellationToken);

            return entities == null
                ? Result<IReadOnlyList<DropTable>>.NotFound()
                : Result<IReadOnlyList<DropTable>>.Success(entities);
        }
    }
}
