using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BlobStorageTrigger
{
    public class FunctionBlobTrigger
    {
        private readonly ILogger<FunctionQueue> _logger;

        public FunctionBlobTrigger(ILogger<FunctionQueue> logger)
        {
            _logger = logger;
        }

        [Function(nameof(FunctionBlobTrigger))]
        public async Task Run([BlobTrigger("samples-workitems/{name}", Connection = "StorageConnection")] Stream myBlob, string name)
        {
            try
            {
                using (StreamReader reader = new StreamReader(myBlob))
                {
                    var message = reader.ReadToEnd();
                    _logger.LogInformation("File detected");
                }
            }
            catch (Exception error)
            {
                _logger.LogInformation(error.Message);
            }
        }
    }
}
