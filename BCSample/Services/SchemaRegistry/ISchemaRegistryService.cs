using BCSample.Events;
using Confluent.SchemaRegistry;

namespace BCSample.Services.SchemaRegistry
{
    public interface ISchemaRegistryService
    {
        ISchemaRegistryClient _schemaRegistryClient { get; }
        Task<int> RegisterSchemaAsync(string topic, string schemaString);
        Task<string> GetSchemaAsync(int schemaId);
        Task DeleteSchemaAsync(int schemaId);
        Task<RegisteredSchema> GetLatestSchemaAsync(string subject);
        Task<List<int>> GetSubjectVersionsAsync(string subject);
        Task<RegisteredSchema> LookupSchemaAsync(string subject, Schema schema, bool ignoreDeletedSchemas, bool normalize = false);
        Task<List<string>> GetAllSubjectAsync();
        Task<List<int>> GetSubjectVersions(string subject);
        Task<int> RegisterSchemaAsync(IEvent @event);
        Task<string> GetSchemaByIdAsync(int schemaId);
    }
}



