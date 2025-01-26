using EasyNetQ;
using FakeSource;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddEasyNetQ(
    builder.Configuration?.GetConnectionString("RabbitMQConnection") ?? string.Empty);
var host = builder.Build();
host.Run();
