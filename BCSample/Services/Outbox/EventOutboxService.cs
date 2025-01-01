using BCSample.Data;
using BCSample.Events;
using BCSample.Services.SchemaRegistry;
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


            var jsonPayload = JsonSerializer.Serialize(eventData);


            var outboxMessage = new BCSample.Data.Outbox(
                eventData.GetType().Name!,
                typeof(LoginActionEvent).FullName!,
                jsonPayload,
                schemaId
            );


            await SaveToOutboxAsync(outboxMessage);
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
    }
}



