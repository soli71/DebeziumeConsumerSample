using Microsoft.Extensions.Hosting;

public class ConsumerHostedService : IHostedService
{
    private readonly IKafkaConsumerService _consumerService;

    public ConsumerHostedService(IKafkaConsumerService consumerService)
    {
        _consumerService = consumerService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _consumerService.StartConsuming(cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _consumerService.StopConsuming();
        return Task.CompletedTask;
    }
}
