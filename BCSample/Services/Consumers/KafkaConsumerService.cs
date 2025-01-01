using BCSample.Events;
using BCSample.Services.Outbox;
using BCSample.Services.SchemaRegistry;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;

namespace BCSample.Services.Consumers
{
    public class KafkaConsumerService : IKafkaConsumerService, IDisposable
    {
        private readonly IConsumer<string, LoginActionEvent> _consumer;
        private readonly ILogger<KafkaConsumerService> _logger;
        private readonly ISchemaRegistryService _schemaRegistryService;
        private bool _isConsuming;

        public KafkaConsumerService(
            ILogger<KafkaConsumerService> logger, ISchemaRegistryService schemaRegistryService)
        {
            _logger = logger;

            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:29092",
                GroupId = "my-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                SecurityProtocol = SecurityProtocol.Plaintext,
            };
            _schemaRegistryService = schemaRegistryService;
            _consumer = new ConsumerBuilder<string, LoginActionEvent>(config)
                .SetValueDeserializer(new AvroDeserializer<LoginActionEvent>(_schemaRegistryService._schemaRegistryClient).AsSyncOverAsync())
                .SetErrorHandler((_, e) => _logger.LogError($"Error: {e.Reason}"))
                .Build();
     
        }

        public async Task StartConsumingAsync(CancellationToken cancellationToken)
        {
            var topic = "your-topic-name";
            _consumer.Subscribe(topic);
            _isConsuming = true;

            try
            {
                while (_isConsuming && !cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = _consumer.Consume(cancellationToken);

                        if (consumeResult?.Message == null) continue;

                        _logger.LogInformation(
                            "Received message: Topic: {Topic} Partition: {Partition} Offset: {Offset}",
                            consumeResult.Topic,
                            consumeResult.Partition,
                            consumeResult.Offset
                        );

                        _consumer.Commit(consumeResult);
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError($"Consume error: {ex.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Consuming cancelled");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while consuming messages");
            }
            finally
            {
                _consumer.Close();
            }
        }

       

        public Task StopConsumingAsync()
        {
            _isConsuming = false;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _consumer?.Dispose();
        }
    }
}
