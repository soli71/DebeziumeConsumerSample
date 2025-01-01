using BCSample.Events;
using Confluent.SchemaRegistry;
using System.Runtime.InteropServices;

namespace BCSample.Services.SchemaRegistry
{
    public class SchemaRegistryService : ISchemaRegistryService
    {
        public  ISchemaRegistryClient _schemaRegistryClient { get; }
        public SchemaRegistryService()
        {
            var schemaRegistryParams = new Dictionary<string, string>
            {
                { "schema.registry.url", "http://localhost:8081" },
            };

            _schemaRegistryClient = new CachedSchemaRegistryClient(schemaRegistryParams);
        }
        public Task DeleteSchemaAsync(int schemaId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<string>> GetAllSubjectAsync()
        {
            var result = await _schemaRegistryClient.GetAllSubjectsAsync();
            return result;
        }
        public async Task<List<int>> GetSubjectVersions(string subject)
        {
            var result = await _schemaRegistryClient.GetSubjectVersionsAsync(subject);
            return result;
        }
        public async Task<RegisteredSchema> GetLatestSchemaAsync(string subject)
        {
            var result = await _schemaRegistryClient.GetLatestSchemaAsync(subject);
            return result;
        }

        public async Task<string> GetSchemaAsync(int schemaId)
        {
            var result = await _schemaRegistryClient.GetSchemaAsync(schemaId);
            return result;
        }

        public async Task<List<int>> GetSubjectVersionsAsync(string subject)
        {
            var result = await _schemaRegistryClient.GetSubjectVersionsAsync(subject);
            return result;
        }

        public async Task<RegisteredSchema> LookupSchemaAsync(string subject, Schema schema, bool ignoreDeletedSchemas, bool normalize = false)
        {
            var result = await _schemaRegistryClient.LookupSchemaAsync(subject, schema, ignoreDeletedSchemas, normalize);
            return result;
        }

        public async Task<int> RegisterSchemaAsync(string topic, string schemaString)
        {
            var schemaId = await _schemaRegistryClient.RegisterSchemaAsync(topic, schemaString);
            return schemaId;
        }

        public async Task<bool> IsCompatibleAsync(string subject, string avroSchema)
        {
            var result = await _schemaRegistryClient.IsCompatibleAsync(subject, avroSchema);
            return result;
        }


        public async Task<int> RegisterSchemaAsync(IEvent @event)
        {
            try
            {
                var subject = @event.GetType().Name;
                var schemaString = @event.Schema.ToString();

                try
                {
                    var registeredSchema = await LookupSchemaAsync(
                        subject,
                        new Schema(
                            schemaType: SchemaType.Avro,
                            schemaString: schemaString,
                            references: null
                        ),
                        ignoreDeletedSchemas: true
                    );

                    return registeredSchema.Id;
                }
                catch (SchemaRegistryException ex) when (ex.ErrorCode == 40403)
                {
                    var schemaId = await RegisterSchemaAsync(subject, schemaString);
                    return schemaId;
                }
                catch (SchemaRegistryException ex) when (ex.ErrorCode == 40401)
                {
                    var schemaId = await RegisterSchemaAsync(subject, schemaString);
                    return schemaId;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to register schema", ex);
            }
        }
    }
}



