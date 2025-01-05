using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using CarShareBackground;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration(config =>
    {
        config.AddUserSecrets<ProcessDriversLicensePhoto>(optional: true, reloadOnChange: false);
    })
    .Build();

host.Run();