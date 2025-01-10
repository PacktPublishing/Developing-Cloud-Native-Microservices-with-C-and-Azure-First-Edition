using EasyNetQ;
using Microsoft.VisualBasic;
using SharedMessages.BasicTypes;
using SharedMessages.RouteNegotiation;
using System.Net.Http.Headers;

namespace FakeSource
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IBus _bus;
        public Worker(ILogger<Worker> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var requestUser1 = new UserBasicInfoMessage
                { Id = Guid.NewGuid(), DisplayName = "RequestUser1" };
            var requestUser2 = new UserBasicInfoMessage
                { Id = Guid.NewGuid(), DisplayName = "RequestUser2" };
            var offerUser = new UserBasicInfoMessage
                { Id = Guid.NewGuid(), DisplayName = "RequestUser2" };

            var offerDate = DateTime.Now.AddDays(10);
            var requestDateInterval = new TimeIntervalMessage
                { Start = offerDate.AddDays(-2), End = offerDate.AddDays(2) };

            var town1 = new TownBasicInfoMessage
            {
                Id = Guid.NewGuid(),
                Name = "Phoenix",
                Location = new GeoLocalizationMessage { Latitude = 33.5722, Longitude = -112.0892 }
            };
            var town2 = new TownBasicInfoMessage
            {
                Id = Guid.NewGuid(),
                Name = "Santa Fe",
                Location = new GeoLocalizationMessage { Latitude = 35.6619, Longitude = -105.982 }
            };
            var town3 = new TownBasicInfoMessage
            {
                Id = Guid.NewGuid(),
                Name = "Cheyenne",
                Location = new GeoLocalizationMessage { Latitude = 41.135, Longitude = -104.79 }
            };
            var request1 = new RouteRequestMessage
            {
                Id= Guid.NewGuid(),
                TimeStamp =1,
                Source=town1,
                Destination = town2,
                User = requestUser1,
                When = requestDateInterval
            };
            var request2 = new RouteRequestMessage
            {
                Id = Guid.NewGuid(),
                TimeStamp = 1,
                Source = town2,
                Destination = town3,
                User = requestUser2,
                When = requestDateInterval
            };
            var offerMessage = new RouteOfferMessage
            {
                Id = Guid.NewGuid(),
                TimeStamp = 1,
                User = offerUser,
                When = offerDate,
                Path = new List<TownBasicInfoMessage> 
                    { town1, town2, town3 }
            };
            var extendedMessage = new RouteExtendedMessage
            {
                TimeStamp = 2,
                ExtendedRoute = offerMessage,
                AddedRequests = new List<RouteRequestMessage>
                {request1,request2 }
            };
            var delayInterval = 5000;
            await Task.Delay(delayInterval, stoppingToken);
            await _bus.PubSub.PublishAsync<RouteRequestMessage>(request1);
            await Task.Delay(delayInterval, stoppingToken);
            await _bus.PubSub.PublishAsync<RouteOfferMessage>(offerMessage);
            await Task.Delay(delayInterval, stoppingToken);
            await _bus.PubSub.PublishAsync<RouteRequestMessage>(request2);
            await Task.Delay(2*delayInterval, stoppingToken);
            await _bus.PubSub.PublishAsync<RouteExtendedMessage>(extendedMessage);
            stoppingToken.WaitHandle.WaitOne();
        }
    }
}
