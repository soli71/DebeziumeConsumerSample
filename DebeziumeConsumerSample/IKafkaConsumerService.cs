public interface IKafkaConsumerService
{
    void StartConsuming(CancellationToken cancellationToken);
    void StopConsuming();
}
