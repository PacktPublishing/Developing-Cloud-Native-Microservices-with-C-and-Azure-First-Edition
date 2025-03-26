using DBDriver.Extensions;
using DDD.ApplicationLayer;
using EasyNetQ;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Hosting;
using RoutesPlanning.HostedServices;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.AddServiceDefaults();
builder.Services.AddOpenApi();
builder.Services.AddApplicationServices();
builder.Services.AddDbDriver(
    builder.Configuration?.GetConnectionString("DefaultConnection") ?? string.Empty);
builder.Services.AddEasyNetQ(
    builder.Configuration?.GetConnectionString("RabbitMQConnection")??string.Empty)
    .UseAlwaysNackWithRequeueConsumerErrorStrategy();
builder.Services.AddHostedService<OutputSendingService>();
builder.Services.AddHostedService<HouseKeepingService>();
builder.Services.AddHostedService<MainService>();

var app = builder.Build();
app.MapDefaultEndpoints();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}



app.MapGet("/liveness", () =>
{
    if (MainService.ErrorsCount < 6) return Results.Ok();
    else return Results.InternalServerError();
})
.WithName("GetLiveness");

app.Run();

