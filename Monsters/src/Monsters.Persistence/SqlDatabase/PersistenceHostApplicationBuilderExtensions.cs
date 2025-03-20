using Common.Infrastructure.Persistence;
using Common.Infrastructure.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Monsters.Core.Domain;
using Monsters.Persistence.Repositories;
using Monsters.Persistence.SqlDatabase.Options;

namespace Monsters.Persistence.SqlDatabase
{
    public static class PersistenceHostApplicationBuilderExtensions
    {
        public static IHostApplicationBuilder AddMonstersDbContext(
            this IHostApplicationBuilder builder,
            Action<MonstersDbSqlOptions> config)
        {
            builder.Services
                .AddOptions<MonstersDbSqlOptions>()
                .Configure(config);

            builder.Services
                .AddDbContextFactory<MonstersDbContext>((serviceProvider, builder) =>
                {
                    var sqlOptions = serviceProvider.GetRequiredService<IOptions<MonstersDbSqlOptions>>();

                    builder.UseSqlServer(sqlOptions.Value.ConnectionString, options =>
                    {
                        options.MigrationsAssembly(typeof(MonstersDbContext).Assembly.GetName().Name);
                        options.MigrationsHistoryTable($"__{nameof(MonstersDbContext)}");

                        options.EnableRetryOnFailure(5);
                        options.MinBatchSize(1);
                    });
                })
                .AddDbContext<MonstersDbContext>();

            return builder;
        }

        public static IHostApplicationBuilder AddMonstersRepositories(
            this IHostApplicationBuilder builder,
            Action<MonstersDbSqlOptions> config)
        {
            builder.AddMonstersDbContext(config);

            builder.Services
                .AddScoped<IRepository<Monster>, EFRepository<Monster, MonstersDbContext>>()
                .AddScoped<IRepository<Item>, EFRepository<Item, MonstersDbContext>>()
                .AddScoped<IRepository<DropTable>, DropTableRepository>();

            return builder;
        }
    }
}
