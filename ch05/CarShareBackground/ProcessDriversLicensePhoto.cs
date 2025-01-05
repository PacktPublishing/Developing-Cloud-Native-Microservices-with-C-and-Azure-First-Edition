using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CarShareBackground
{
    public class ProcessDriversLicensePhoto
    {
        private readonly ILogger _logger;

        public ProcessDriversLicensePhoto(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ProcessDriversLicensePhoto>();
        }

        [Function(nameof(ProcessDriversLicensePhoto))]
        public async Task Run([BlobTrigger("drivers-license/{name}", Connection = "CarShareStorage")] Stream myBlob, string name)
        {
            StreamReader reader = new StreamReader(myBlob);
            var message = reader.ReadToEnd();
            _logger.LogInformation("File detected");
        }
    }
}
