using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Monsters.Application.MessageProcessor;
using Monsters.Persistence.SqlDatabase;

namespace Monsters.Integration.Tests
{
    public class MonstersMessageProcessorApplicationFactory : WebApplicationFactory<Program>
    {
        private const string DbPath = "DataSource=C:\\Souls\\SoulsServices\\sqlite\\monstersdatabase.db";
        private bool disposed = false;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureTestServices(services =>
            {
                RemovePublishStartedMessageService(services);
                ReplaceExistingSqlDatabaseWithSqlite(services);

                services.AddMassTransitTestHarness(config =>
                {
                    config.SetTestTimeouts(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
                });
            })
            // This is needed to run as a WebApplicationFactory
            .Configure(app => { });
        }

        private static void ReplaceExistingSqlDatabaseWithSqlite(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MonstersDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContextFactory<MonstersDbContext>(options =>
            {
                options.UseSqlite(DbPath);
            });
        }

        private static void RemovePublishStartedMessageService(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IHostedService) &&
                     d.ImplementationType == typeof(PublishStartedMessageWorker));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            var host = base.CreateHost(builder);

            using (var scope = host.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MonstersDbContext>();
                db.Database.EnsureDeleted();
                var isCreated = db.Database.EnsureCreated();

                if (!isCreated)
                {
                    throw new InvalidOperationException("sqlite database could not be created");
                }
            }

            return host;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;
                using (var scope = Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<MonstersDbContext>();
                    var isDeleted = db.Database.EnsureDeleted();

                    if (!isDeleted)
                    {
                        throw new InvalidOperationException("sqlite database could not be deleted");
                    }
                }
            }            

            base.Dispose(disposing);            
        }
    }
}
