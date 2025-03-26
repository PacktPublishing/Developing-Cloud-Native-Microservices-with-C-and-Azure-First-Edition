using Aspire.Hosting;
using static System.Net.WebRequestMethods;

var builder = DistributedApplication.CreateBuilder(args);
var fakeDestination=builder.AddProject<Projects.FakeDestination>("fakedestination", "FakeDestination");
var routesPlanning = builder.AddProject<Projects.RoutesPlanning>("routesplanning", "http")
    .WaitFor(fakeDestination);
builder.AddProject<Projects.FakeSource>("fakesource", "FakeSource")
    .WaitFor(routesPlanning);
builder.Build().Run();
