namespace Presentation.Persistence.Elasticsearch.Options;

public class ElasticsearchOptions
{
    public string ConnectionString { get; set; } = string.Empty;

    public string DropTableIndex { get; set; } = "drop-tables";

    public string ItemIndex { get; set; } = "items";

    public string MonsterIndex { get; set; } = "monsters";
}
