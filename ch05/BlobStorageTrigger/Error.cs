using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BlobStorageTrigger
{
    public class Error
    {
        private readonly ILogger<Error> _logger;

        public Error(ILogger<Error> logger)
        {
            _logger = logger;
        }

        [Function(nameof(Error))]
        public async Task Run([BlobTrigger("samples-workitems-error/{name}", Connection = "Teste")] Stream stream, string name)
        {
            using var blobStreamReader = new StreamReader(stream);
            var content = await blobStreamReader.ReadToEndAsync();
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} \n Data: {content}");
        }
    }
}
