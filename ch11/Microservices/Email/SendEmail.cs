using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Email
{
    public static class SendEmail
    {
        [FunctionName(nameof(SendEmail))]
        public static async Task<HttpResponseMessage> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestMessage req,
            ILogger log)
        {
            var requestData = await req.Content.ReadAsStringAsync();

            var connectionString = Environment.GetEnvironmentVariable("EmailQueueConnectionString");

            var storageAccount = CloudStorageAccount.Parse(connectionString);

            var queueClient = storageAccount.CreateCloudQueueClient();

            var messageQueue = queueClient.GetQueueReference("email");

            var message = new CloudQueueMessage(requestData);

            await messageQueue.AddMessageAsync(message);

            log.LogInformation("HTTP trigger from SendEmail function processed a request.");
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new { success = true }), Encoding.UTF8, "application/json"),
            };
        }
    }
    
}
