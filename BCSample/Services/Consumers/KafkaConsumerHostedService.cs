namespace BCSample.Services.Consumers
{
    public class KafkaConsumerHostedService : BackgroundService
    {
        private readonly IKafkaConsumerService _kafkaConsumerService;
        private readonly ILogger<KafkaConsumerHostedService> _logger;

        public KafkaConsumerHostedService(
            IKafkaConsumerService kafkaConsumerService,
            ILogger<KafkaConsumerHostedService> logger)
        {
            _kafkaConsumerService = kafkaConsumerService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await _kafkaConsumerService.StartConsumingAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred executing the consumer service");
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _kafkaConsumerService.StopConsumingAsync();
            await base.StopAsync(cancellationToken);
        }
    }
}
