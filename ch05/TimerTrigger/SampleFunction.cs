using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TimerTrigger
{
    public class SampleFunction
    {
        private readonly ILogger _logger;

        public SampleFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SampleFunction>();
        }

        [Function("SampleFunction")]
        public void Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            
            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
