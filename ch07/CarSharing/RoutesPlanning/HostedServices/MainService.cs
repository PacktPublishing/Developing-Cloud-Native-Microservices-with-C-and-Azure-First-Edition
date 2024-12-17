
using DDD.ApplicationLayer;
using EasyNetQ;
using Microsoft.IdentityModel.Tokens;
using RoutesPlanningApplicationServices.Commands;
using SharedMessages.RouteNegotiation;

namespace RoutesPlanning.HostedServices
{
    public class MainService(IConfiguration configuration, IBus bus,  IServiceProvider services): BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var routeOfferSubscription = await bus.PubSub.SubscribeAsync<RouteOfferMessage>(
                SubscriptionId<RouteOfferMessage>(),SafeProcessMessage, stoppingToken);
            var routeClosedAbortedSubscription = await bus.PubSub.SubscribeAsync<RouteClosedAbortedMessage>(
                SubscriptionId<RouteClosedAbortedMessage>(), SafeProcessMessage, stoppingToken);
            var routeExtendedSubscription = await bus.PubSub.SubscribeAsync<RouteExtendedMessage>(
                SubscriptionId<RouteExtendedMessage>(), SafeProcessMessage, stoppingToken);
            var routeRequestSubscription = await bus.PubSub.SubscribeAsync<RouteRequestMessage>(
                SubscriptionId<RouteRequestMessage>(), SafeProcessMessage, stoppingToken);
            
            stoppingToken.WaitHandle.WaitOne();

            routeRequestSubscription.Dispose();
            routeExtendedSubscription.Dispose();
            routeClosedAbortedSubscription.Dispose();
            routeOfferSubscription.Dispose();
        }
        protected async Task ProcessMessage<T>(T message)
        {
            using (var scope = services.CreateScope()) 
            {
                var handler=scope.ServiceProvider.GetRequiredService<ICommandHandler<MessageCommand<T>>>();
                await handler.HandleAsync(new MessageCommand<T>(message));
            }
        }
        protected async Task SafeProcessMessage<T>(T message)
        {
            try
            {
                await ProcessMessage(message);
                DeclareSuccessFailure();
            }
            catch 
            {
                DeclareSuccessFailure(true);
                throw;
            }
        }
        private readonly Lock _countErrorsLock = new();
        private static int _errorCount = 0;
        public static int ErrorsCount => _errorCount;
        private void DeclareSuccessFailure(bool isFailure=false)
        {
            using (_countErrorsLock.EnterScope())
            {
                if (isFailure) _errorCount++;
                else _errorCount = 0;
            }
        }
        string SubscriptionId<T>()
        {
            return string.Format("{0}_{1}",
                configuration["Messages__SubscriptionIdPrefix"],
                typeof(T).Name);
        }
    }
}
