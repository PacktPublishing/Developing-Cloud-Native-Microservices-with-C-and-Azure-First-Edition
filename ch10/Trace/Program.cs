using Azure.Monitor.OpenTelemetry.Exporter;
// Using OpenTelemetry Metrics for monitoring
using OpenTelemetry.Metrics;
// Using OpenTelemetry Resources for resource management
using OpenTelemetry.Resources;
// Using OpenTelemetry Trace for tracing
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Retrieve Application Insights connection string from configuration
string appInsightsConnectionString = builder.Configuration["AzureMonitor:ConnectionString"];

builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
{
    tracerProviderBuilder
        // Set resource builder with application name
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(builder.Environment.ApplicationName))
        // Add ASP.NET Core instrumentation
        .AddAspNetCoreInstrumentation()
        // Add HTTP client instrumentation
        .AddHttpClientInstrumentation()
        // Add Azure Monitor Trace Exporter with connection string
        .AddAzureMonitorTraceExporter(options =>
         {
             options.ConnectionString = appInsightsConnectionString;
         });
});

// Add Application Insights only for logging & metrics (without re-adding tracing)
builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = appInsightsConnectionString;
    options.EnableAdaptiveSampling = false; // Disable AI's automatic trace sampling
    options.EnableDependencyTrackingTelemetryModule = false; // Prevents duplicate dependency tracking
    options.EnableRequestTrackingTelemetryModule = false; // Prevents duplicate HTTP request tracking
});

var app = builder.Build();

// Map GET request to /error endpoint
app.MapGet("/error", async (HttpContext context) =>
{
    var httpClient = new HttpClient();
    var response = await httpClient.GetAsync("https://anyhost.sample.com/data");

    return "Hello Trace!";
});

// Map GET request to /success endpoint
app.MapGet("/success", async (HttpContext context) =>
{
    var httpClient = new HttpClient();
    var response = await httpClient.GetAsync("https://www.packtpub.com/");

    return "Hello Trace!";
});

app.Run();