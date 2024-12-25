using Confluent.Kafka;
using Confluent.SchemaRegistry;

public class KafkaConsumerService : IKafkaConsumerService
{
    private readonly IConsumer<string, Rootobject> _consumer;

    public KafkaConsumerService(IDeserializer<Rootobject> deserializer)
    {
        var schemaRegistryConfig = new SchemaRegistryConfig
        {
            Url = "http://localhost:8081"
        };

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = "localhost:9093",
            GroupId = "my-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);
        _consumer = new ConsumerBuilder<string, Rootobject>(consumerConfig)
            .SetValueDeserializer(deserializer)
            .Build();
    }

    public void StartConsuming(CancellationToken cancellationToken)
    {
        _consumer.Subscribe("test.DebeziumDb.dbo.Student");

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(cancellationToken);
                    var envelope = consumeResult.Message.Value;

                    var snapshot = envelope.payload.source.snapshot;
                    Console.WriteLine($"Key: {consumeResult.Message.Key}, Id: {envelope.payload.after.Id}  , Name : {envelope.payload.after.Name}");
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Consume error: {e.Error.Reason}");
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Handle cancellation
        }
        finally
        {
            _consumer.Close();
        }
    }

    public void StopConsuming()
    {
        _consumer.Close();
    }
}
