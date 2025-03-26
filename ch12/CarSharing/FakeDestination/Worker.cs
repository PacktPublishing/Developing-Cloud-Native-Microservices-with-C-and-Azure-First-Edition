using EasyNetQ;
using SharedMessages.RouteNegotiation;
using System.Text.Json;

namespace FakeDestination
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IBus _bus;
        public Worker(ILogger<Worker> logger, IBus bus)
        {
            _logger = logger;
            _bus= bus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var routeExtensionProposalSubscription = await _bus.PubSub.SubscribeAsync<RouteExtensionProposalsMessage>(
                "FakeDestination", 
                m =>
                {
                    var toPrint=JsonSerializer.Serialize(m);
                    _logger.LogInformation("Message received: {0}", toPrint);
                }, 
                stoppingToken);

            await Task.Delay(Timeout.Infinite, stoppingToken);

            routeExtensionProposalSubscription.Dispose();
        }
    }
}
