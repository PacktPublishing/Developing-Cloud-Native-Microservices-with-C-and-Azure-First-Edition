using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace RoutesPlanner
{
    public class CarSeeking
    {
        private readonly ILogger<CarSeeking> _logger;

        public CarSeeking(ILogger<CarSeeking> logger)
        {
            _logger = logger;
        }

        [Function(nameof(CarSeeking))]
        public async Task Run(
            [ServiceBusTrigger("car-seeking-requests", "routes", Connection = "car-share-bus")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

             // Complete the message
            await messageActions.CompleteMessageAsync(message);
        }
    }
}
