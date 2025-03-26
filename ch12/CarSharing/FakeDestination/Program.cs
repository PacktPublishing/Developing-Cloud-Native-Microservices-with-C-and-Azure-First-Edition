using EasyNetQ;
using FakeDestination;

var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();
builder.Services.AddEasyNetQ(
    builder.Configuration?.GetConnectionString("RabbitMQConnection") ?? string.Empty);
var host = builder.Build();


host.Run();
