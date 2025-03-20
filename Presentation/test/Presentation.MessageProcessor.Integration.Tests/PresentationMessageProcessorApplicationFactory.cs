using Common.Testing.Integration.Containers;
using Elastic.Clients.Elasticsearch;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Application.MessageProcessor;

namespace Presentation.MessageProcessor.Integration.Tests
{
    public class PresentationMessageProcessorApplicationFactory : WebApplicationFactory<Program>
    {
        public ElasticsearchContainer ElasticsearchContainer { get; set; } = null!;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureTestServices(services =>
            {
                ReplaceElasticsearchRepositories(services);

                services.AddMassTransitTestHarness(config =>
                {
                    config.SetTestTimeouts(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
                });
            })
            // This is needed to run as a WebApplicationFactory
            .Configure(app => { });
        }

        private void ReplaceElasticsearchRepositories(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ElasticsearchClient));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddSingleton(_ => ElasticsearchContainer.ElasticsearchClient);
        }
    }
}
