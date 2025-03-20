using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Elastic.Clients.Elasticsearch.Mapping;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Presentatin.Application.MessageProcessor.Messages;
using Presentation.Core.DataModels;
using Presentation.Persistence.Elasticsearch.Options;

namespace Presentatin.Application.MessageProcessor.MessageHandlers
{
    public class EnsureElasticsearchDbCreatedWhenMessageProcessorStarted : IConsumer<PresentationMessageProcessorStarted>
    {
        private readonly ILogger<EnsureElasticsearchDbCreatedWhenMessageProcessorStarted> logger;
        private readonly ElasticsearchClient elasticSearchClient;
        private readonly IOptions<ElasticsearchOptions> options;

        public EnsureElasticsearchDbCreatedWhenMessageProcessorStarted(
            ILogger<EnsureElasticsearchDbCreatedWhenMessageProcessorStarted> logger,
            ElasticsearchClient elasticSearchClient,
            IOptions<ElasticsearchOptions> options)
        {
            this.logger = logger;
            this.elasticSearchClient = elasticSearchClient;
            this.options = options;
        }

        public async Task Consume(ConsumeContext<PresentationMessageProcessorStarted> context)
        {
            await CreateIndex<DropTableDetail>(options.Value.DropTableIndex);
            await CreateIndex<ItemDetail>(options.Value.ItemIndex);
            await CreateIndex<MonsterDetail>(options.Value.MonsterIndex);
        }

        private async Task CreateIndex<T>(string indexName)
        {
            logger.LogInformation($"ensuring {indexName} elastic index exists");
            var getRequest = new GetIndexRequest(indexName);
            var getResponse = await elasticSearchClient.Indices.GetAsync(getRequest);

            if (getResponse.Indices.Keys.Contains(indexName))
            {
                return;
            }

            var request = new CreateIndexRequestDescriptor<T>(indexName);
            request.Mappings(map => map.Properties(new Properties<T>()
                {
                    { "identifier", new KeywordProperty() }
                }));
            var response = await elasticSearchClient.Indices.CreateAsync(request);

            if (response.IsSuccess() == false)
            {
                var error = response.ApiCallDetails.DebugInformation;
                throw new Exception(error);
            }
        }
    }
}
