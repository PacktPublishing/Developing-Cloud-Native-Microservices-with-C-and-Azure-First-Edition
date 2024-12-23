using CarShareBackground;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration(config =>
    {
        config.AddUserSecrets<ProcessDriversLicensePhoto>(optional: true, reloadOnChange: false);
    })
    .Build();

host.Run();