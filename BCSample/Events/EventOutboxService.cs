using Avro;
using BCSample.Data;
using BCSample.Partials;
using Confluent.SchemaRegistry;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BCSample.Events
{
    public class LoginActionEventSchema
    {
        // Avro schema definition
        private static readonly string SchemaJson = @"{
        ""type"": ""record"",
        ""name"": ""LoginActionEvent"",
        ""namespace"": ""BCSample.Events"",
        ""fields"": [
            { ""name"": ""UserName"", ""type"": ""string"" },
            { ""name"": ""LoginTime"", ""type"": { ""type"": ""long"", ""logicalType"": ""timestamp-millis"" }}
        ]
    }";

        public static RecordSchema Schema => (RecordSchema)RecordSchema.Parse(SchemaJson);
    }
    public class EventOutboxService : IEventOutboxService
    {
        private readonly ISchemaRegistryClient _schemaRegistryClient;
        private readonly DbContext _dbContext;

        public EventOutboxService(ISchemaRegistryClient schemaRegistryClient, DbContext dbContext)
        {
            _schemaRegistryClient = schemaRegistryClient;
            _dbContext = dbContext;
        }

        public async Task SaveEventToOutboxAsync(LoginActionEvent eventData, string topic)
        {

            var schemaId = await RegisterSchemaAsync(topic);


            var jsonPayload = JsonSerializer.Serialize(eventData);


            var outboxMessage = new Outbox(
                typeof(LoginActionEvent).FullName!,
                jsonPayload,
                schemaId
            );


            await SaveToOutboxAsync(outboxMessage);
        }

        private async Task<int> RegisterSchemaAsync(string topic)
        {
            try
            {
                var subject = $"{topic}-value";
                var schema = LoginActionEventSchema.Schema.ToString();

                var registeredSchema = await _schemaRegistryClient.RegisterSchemaAsync(subject, schema);

                return registeredSchema;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to register schema", ex);
            }
        }

        private async Task SaveToOutboxAsync(Outbox message)
        {
            try
            {
                _dbContext.Set<Outbox>().Add(message);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to save to outbox", ex);
            }
        }
    }
}



