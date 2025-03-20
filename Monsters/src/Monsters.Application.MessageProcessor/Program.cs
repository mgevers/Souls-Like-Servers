using Common.Infrastructure.OpenTelemetry;
using Common.Infrastructure.ServiceBus.MassTransit;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Monsters.Application.MessageProcessor.MessageHandlers;
using Monsters.Core.CommandHandlers.Items;
using Monsters.Persistence.SqlDatabase;
using Monsters.Persistence.SqlDatabase.Options;

namespace Monsters.Application.MessageProcessor
{
    public partial class Program
    {
        public static Task Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder
                .AddMonstersRepositories(options => builder.Configuration.GetSection(nameof(MonstersDbSqlOptions)).Bind(options))
                .SetupMassTransit(
                    rabbitOptions =>
                    {
                        builder.Configuration.GetSection(nameof(RabbitMqTransportOptions)).Bind(rabbitOptions);
                    },
                    busConfig =>
                    {
                        busConfig.AddConsumers(typeof(EnsureMonstersDbCreatedWhenMessageProcessorStarted).Assembly);
                        busConfig.AddConsumers(typeof(AddItemCommandHandler).Assembly);
                    })
                .UseSerilog()
                .ConfigureOpenTelemetry(serviceName: "monsters-message-processor");

            builder.Services
                .AddHostedService<PublishStartedMessageWorker>();

            return builder.Build().RunAsync();
        }
    }
}
