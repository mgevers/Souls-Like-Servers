using Common.Infrastructure.Persistence;
using Common.Infrastructure.Persistence.Elasticsearch;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Presentation.Core.DataModels;
using Presentation.Persistence.Elasticsearch.Options;

namespace Presentation.Persistence.Elasticsearch
{
    public static class PersistenceHostApplicationBuilderExtensions
    {
        public static IHostApplicationBuilder AddElasticsearchClient(this IHostApplicationBuilder builder, Action<ElasticsearchOptions> config)
        {
            builder.Services
                .AddOptions<ElasticsearchOptions>()
                .Configure(config);

            builder.Services.AddSingleton(sp =>
            {
                var options = sp.GetRequiredService<IOptions<ElasticsearchOptions>>();
                var settings = new ElasticsearchClientSettings(new Uri(options.Value.ConnectionString));
                settings.ServerCertificateValidationCallback(CertificateValidations.AllowAll);
                settings.DisableDirectStreaming();

                return new ElasticsearchClient(settings);
            });

            return builder;
        }

        public static IHostApplicationBuilder AddPresentationRepositories(this IHostApplicationBuilder builder, Action<ElasticsearchOptions> config)
        {
            builder
                .AddElasticsearchClient(config);

            builder.Services
                .AddSingleton<IRepository<DropTableDetail>>(sp =>
                {
                    var options = sp.GetRequiredService<IOptions<ElasticsearchOptions>>();
                    var client = sp.GetRequiredService<ElasticsearchClient>();

                    return new ElasticsearchRepository<DropTableDetail>(client, options.Value.DropTableIndex);
                })
                .AddSingleton<IRepository<ItemDetail>>(sp =>
                {
                    var options = sp.GetRequiredService<IOptions<ElasticsearchOptions>>();
                    var client = sp.GetRequiredService<ElasticsearchClient>();

                    return new ElasticsearchRepository<ItemDetail>(client, options.Value.ItemIndex);
                })
                .AddSingleton<IRepository<MonsterDetail>>(sp =>
                {
                    var options = sp.GetRequiredService<IOptions<ElasticsearchOptions>>();
                    var client = sp.GetRequiredService<ElasticsearchClient>();

                    return new ElasticsearchRepository<MonsterDetail>(client, options.Value.MonsterIndex);
                });

            return builder;
        }
    }
}
