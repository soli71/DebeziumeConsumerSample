namespace BCSample.Services.Consumers
{
    public interface IKafkaConsumerService
    {
        Task StartConsumingAsync(CancellationToken cancellationToken);
        Task StopConsumingAsync();
    }
}
