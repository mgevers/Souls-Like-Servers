using Common.Infrastructure;
using Common.Infrastructure.Auth;
using Common.Infrastructure.Auth.Token;
using Common.Infrastructure.ServiceBus.MassTransit;
using MassTransit;
using Monsters.Core.Commands.Monsters;
using Presentation.Application.API.EventHandlers.Monsters;
using Presentation.Application.API.Hubs;
using Presentation.Persistence.Elasticsearch;
using Presentation.Persistence.Elasticsearch.Options;
using System.Text.Json;

namespace Presentation.Application.API
{
    public partial class Program
    {
        public static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddCommonConfiguration();
            builder
                .AddPresentationRepositories(options => builder.Configuration.GetSection(nameof(ElasticsearchOptions)).Bind(options))
                .SetupMassTransit(
                    rabbitOptions =>
                    {
                        builder.Configuration.GetSection(nameof(RabbitMqTransportOptions)).Bind(rabbitOptions);
                    },
                    busConfig =>
                    {
                        busConfig.AddRequestClient<AddMonsterCommand>();
                        busConfig.AddRequestClient<UpdateMonsterCommand>();
                        busConfig.AddRequestClient<RemoveMonsterCommand>();

                        busConfig.AddConsumer<NotifyClientWhenMonstersChangedEventHandler>()
                            .Endpoint(e =>
                            {
                                e.Temporary = true;
                                e.InstanceId = Guid.NewGuid().ToString();
                            });
                    });

            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();
            ConfigureApplication(app);

            return app.RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddSignalR();

            services
                .AddJwtAuthentication(options => configuration.GetSection(nameof(OAuthOptions)).Bind(options))                
                .AddMediatR(config => config.RegisterServicesFromAssemblies(typeof(Program).Assembly));
        }

        private static void ConfigureApplication(WebApplication app)
        {
            app.AddCommonConfiguration();
            app.MapControllers();
            app.MapHub<PresentationHub>("/api/presentation/presentation-client");
        }
    }
}
