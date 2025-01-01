using Avro;
using Avro.IO;
using Avro.Specific;
using BCSample.Data;
using BCSample.Events;
using BCSample.Services.SchemaRegistry;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BCSample.Services.Outbox
{

    public class EventOutboxService : IEventOutboxService
    {
        private readonly ISchemaRegistryService _schemaRegistryService;
        private readonly ApplicationDbContext _dbContext;

        public EventOutboxService(ApplicationDbContext dbContext, ISchemaRegistryService schemaRegistryService)
        {

            _dbContext = dbContext;
            _schemaRegistryService = schemaRegistryService;
        }

        public async Task SaveEventToOutboxAsync(IEvent eventData)
        {

            var schemaId = await _schemaRegistryService.RegisterSchemaAsync(eventData);

            using var memoryStream = new MemoryStream();
            var encoder = new BinaryEncoder(memoryStream);
            var writer = new SpecificDatumWriter<IEvent>(eventData.Schema);
            writer.Write(eventData, encoder);
            var jsonPayload = Convert.ToBase64String(memoryStream.ToArray());
       


            var outboxMessage = new BCSample.Data.Outbox(
                eventData.GetType().Name!,
                typeof(LoginActionEvent).FullName!,
                jsonPayload,
                schemaId
            );


            await SaveToOutboxAsync(outboxMessage);

            await ReadEventFromOutboxAsync(4);
        }



        private async Task SaveToOutboxAsync(BCSample.Data.Outbox message)
        {
            try
            {
                _dbContext.Set<BCSample.Data.Outbox>().Add(message);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to save to outbox", ex);
            }
        }


        public async Task<IEvent?> ReadEventFromOutboxAsync(long messageId)
        {
         
            var outboxMessage = await _dbContext.Outbox
                .FirstOrDefaultAsync(o => o.Id == messageId);

            if (outboxMessage == null)
                throw new Exception($"Outbox message with ID {messageId} not found.");

      
            var schema = await _schemaRegistryService.GetSchemaByIdAsync(outboxMessage.SchemaId);

        
            var readerSchema = Schema.Parse(schema);
            var datumReader = new SpecificDatumReader<LoginActionEvent>(readerSchema, readerSchema);

   
            var payloadBytes = Convert.FromBase64String(outboxMessage.Payload);
            using var memoryStream = new MemoryStream(payloadBytes);
            var decoder = new BinaryDecoder(memoryStream);

            var @event = datumReader.Read(null, decoder) as IEvent;

            return @event;
        }
    }
}



