using System;
using System.Text;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace IoTHubFunction
{
    public class IoTFunction
    {
        private readonly ILogger<IoTFunction> _logger;

        public IoTFunction(ILogger<IoTFunction> logger)
        {
            _logger = logger;
        }

        [Function(nameof(IoTFunction))]
        public void Run([EventHubTrigger("messages/events", Connection = "EventHubConnection")] EventData[] events)
        {
            foreach (EventData @event in events)
            {
                _logger.LogInformation("Event Body: {body}", @event.EventBody);
            }
        }
    }
}
