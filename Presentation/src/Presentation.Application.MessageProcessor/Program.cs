using Common.Infrastructure.OpenTelemetry;
using Common.Infrastructure.ServiceBus.MassTransit;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Presentation.Core.EventHandlers.Items;
using Presentation.Persistence.Elasticsearch;
using Presentation.Persistence.Elasticsearch.Options;

namespace Presentation.Application.MessageProcessor
{
    public partial class Program
    {
        public static Task Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder
                .AddPresentationRepositories(options => builder.Configuration.GetSection(nameof(ElasticsearchOptions)).Bind(options))
                .SetupMassTransit(
                    rabbitOptions =>
                    {
                        builder.Configuration.GetSection(nameof(RabbitMqTransportOptions)).Bind(rabbitOptions);
                    },
                    busConfig =>
                    {
                        busConfig.AddConsumers(typeof(SyncElasticsearchWhenItemAddedEventHandler).Assembly);
                        busConfig.AddConsumers(typeof(Program).Assembly);
                    })
                .UseSerilog()
                .ConfigureOpenTelemetry(serviceName: "presentation-message-processor");

            builder.Services
                .AddHostedService<PublishStartedMessageWorker>();

            return builder.Build().RunAsync();
        }
    }
}
