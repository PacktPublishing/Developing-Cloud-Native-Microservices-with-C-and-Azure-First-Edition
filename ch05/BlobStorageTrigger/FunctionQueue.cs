using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BlobStorageTrigger
{
    public class FunctionQueue
    {
        private readonly ILogger<FunctionQueue> _logger;

        public FunctionQueue(ILogger<FunctionQueue> logger)
        {
            _logger = logger;
        }

        [Function(nameof(FunctionQueue))]
        public void Run([QueueTrigger("myqueue-items", Connection = "StorageConnection")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");
        }
    }
}
