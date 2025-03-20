using Microsoft.EntityFrameworkCore;
using Monsters.Core.Domain;

namespace Monsters.Persistence.SqlDatabase
{
    public class MonstersDbContext : DbContext
    {
        public MonstersDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<DropTable> DropTables => Set<DropTable>();

        public DbSet<Item> Items => Set<Item>();

        public DbSet<Monster> Monsters => Set<Monster>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MonstersDbContext).Assembly);
        }
    }
}
