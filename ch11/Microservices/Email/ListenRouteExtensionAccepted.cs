using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Email
{
    public class ListenRouteExtensionAccepted
    {

        private readonly ILogger<ListenRouteExtensionAccepted> _logger;

        public ListenRouteExtensionAccepted(ILogger<ListenRouteExtensionAccepted> logger)
        {
            _logger = logger;
        }

        [FunctionName(nameof(ListenRouteExtensionAccepted))]
        public async Task Run([BlobTrigger("event-grid-samples/{name}", Source = BlobTriggerSource.EventGrid, Connection = "ConnectionStringName")] Stream stream, string name)
        {
            using var blobStreamReader = new StreamReader(stream);

            var content = await blobStreamReader.ReadToEndAsync();

            var connectionString = Environment.GetEnvironmentVariable("EmailQueueConnectionString");

            var storageAccount = CloudStorageAccount.Parse(connectionString);

            var queueClient = storageAccount.CreateCloudQueueClient();

            var messageQueue = queueClient.GetQueueReference("email");

            var message = new CloudQueueMessage(content);

            await messageQueue.AddMessageAsync(message);

            _logger.LogInformation($"C# Blob Trigger (using Event Grid) processed blob\n Name: {name} \n Data: {content}");
        }

    }
}
