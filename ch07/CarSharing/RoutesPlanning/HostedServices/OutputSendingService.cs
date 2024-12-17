using DDD.ApplicationLayer;
using EasyNetQ;
using Polly;
using RoutesPlanningApplicationServices.Commands;
using SharedMessages.RouteNegotiation;

namespace RoutesPlanning.HostedServices
{
    public class OutputSendingService(IConfiguration configuration, IBus bus,
        IServiceProvider services) : BackgroundService
    {
        readonly int updateBatchCount =
                configuration.GetValue<int>("Timing:OutputBatchCount");
        readonly TimeSpan requeueDelay = TimeSpan.FromMinutes(
                configuration.GetValue<int>("Timing:OutputRequeueDelayMin"));
        readonly TimeSpan circuitBreakDelay = TimeSpan.FromMinutes(
                configuration.GetValue<int>("Timing:OutputCircuitBreakMin"));
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //update interval in milliseconds
            int updateInterval =
                configuration.GetValue<int>("Timing__OutputEmptyDelayMS") ;
            bool queueEmpty = false;
            while (!stoppingToken.IsCancellationRequested)
            {
                while (!queueEmpty && !stoppingToken.IsCancellationRequested)
                {
                    queueEmpty=await SafeInvokeCommand();
                }
                await Task.Delay(updateInterval, stoppingToken);
                queueEmpty = false;
            }
        }
        protected  Task SendMessage(RouteExtensionProposalsMessage message)
        {
            var retryPolicy = Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync(4,
                       retryAttempt => TimeSpan.FromSeconds(Math.Pow(1,
                        retryAttempt)));
            var circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(4, circuitBreakDelay);
            var combinedPolicy = Policy
                .WrapAsync(retryPolicy, circuitBreakerPolicy);
            return combinedPolicy.ExecuteAsync(
                async () => await bus.PubSub.PublishAsync<RouteExtensionProposalsMessage>(message));
            
        }
        protected async Task<bool> InvokeCommand()
        {
            using (var scope = services.CreateScope())
            {
                var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<OutputSendingCommand<RouteExtensionProposalsMessage>>>();
                var command = new OutputSendingCommand<RouteExtensionProposalsMessage>(SendMessage, updateBatchCount, requeueDelay);
                await handler.HandleAsync(command);
                return command.OutPutEmpty;
            }
        }
        protected async Task<bool> SafeInvokeCommand()
        {
            try
            {
                return await InvokeCommand();
            }
            catch
            {
                return true;
            };
        }

    }
}
