
using DDD.ApplicationLayer;
using EasyNetQ;
using RoutesPlanningApplicationServices.Commands;

namespace RoutesPlanning.HostedServices
{
    public class HouseKeepingService(IConfiguration configuration,  
        IServiceProvider services): BackgroundService
    { 
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //update interval in milliseconds
            int updateInterval = 
                configuration.GetValue<int>("Timing:HousekeepingIntervalHours")*3600000;
            int deleteDelayDays =
                configuration.GetValue<int>("Timing:HousekeepingDelayDays");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = services.CreateScope())
                    {
                        var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<HouseKeepingCommand>>();
                        await handler.HandleAsync(new HouseKeepingCommand(deleteDelayDays));
                    }
                }
                catch { }
                await Task.Delay(updateInterval, stoppingToken);
            }
        }
    }
}
