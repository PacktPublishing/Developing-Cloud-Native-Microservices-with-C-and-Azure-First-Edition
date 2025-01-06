using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace My_Best_Matches
{
    public class NewMatchTrigger
    {
        private readonly ILogger<NewMatchTrigger> _logger;

        public NewMatchTrigger(ILogger<NewMatchTrigger> logger)
        {
            _logger = logger;
        }

        [Function(nameof(NewMatchTrigger))]
        public void Run([QueueTrigger("new-match", Connection = "CarSharingStorage")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");
        }
    }
}
