using BlobStorageTrigger;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration(config =>
    {
        config.AddUserSecrets<FunctionBlobTrigger>(optional: true, reloadOnChange: false);
    })
    .Build();

host.Run();
