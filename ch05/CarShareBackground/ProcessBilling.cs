using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CarShareBackground
{
    public class ProcessBilling
    {
        private readonly ILogger _logger;

        public ProcessBilling(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ProcessBilling>();
        }

        /// <summary>
        /// Every hour, between 08:00 AM and 05:59 PM, Monday through Saturday
        /// </summary>
        /// <param name="myTimer"></param>
        [Function("ProcessBilling")]
        public void Run([TimerTrigger("0 0 8-17 * * 1-6")] TimerInfo myTimer)
        {
            _logger.LogInformation($"Time to process billing!");
            _logger.LogInformation($"Execution started at: {DateTime.Now}.");

            // TODO - Code for processing billing

            _logger.LogInformation($"Process billing done: {DateTime.Now}.");
        }
    }

}
