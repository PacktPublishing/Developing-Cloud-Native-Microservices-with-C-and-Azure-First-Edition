using DDD.ApplicationLayer;
using Microsoft.Extensions.Configuration;
using RoutesPlanningDomainLayer.Events;
using RoutesPlanningDomainLayer.Models.OutputQueue;
using RoutesPlanningDomainLayer.Models.Request;
using RoutesPlanningDomainLayer.Models.Route;
using SharedMessages.BasicTypes;
using SharedMessages.RouteNegotiation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutesPlanningApplicationServices.EventHandlers
{
    internal class RequestMatchCandidateEventHandler(
        IRouteOfferRepository offerRepo,
        IOutputQueueRepository queueRepo,
        IConfiguration configuration) : IEventHandler<NewMatchCandidateEvent<RouteRequestAggregate>>
    {
        private RouteRequestMessage PrepareMessage(RouteRequestAggregate m)
            => new RouteRequestMessage
            {
                Id = m.Id,
                TimeStamp = m.TimeStamp,
                User = new UserBasicInfoMessage
                {
                    Id = m.User.Id,
                    DisplayName = m.User.DisplayName,
                },
                Destination = new TownBasicInfoMessage
                {
                    Id = m.Destination.Id,
                    Name = m.Destination.Name,
                    Location = new GeoLocalizationMessage
                    {
                        Longitude = m.Destination.Location.X,
                        Latitude = m.Destination.Location.Y,
                    }
                },
                Source = new TownBasicInfoMessage
                {
                    Id = m.Source.Id,
                    Name = m.Source.Name,
                    Location = new GeoLocalizationMessage
                    {
                        Longitude = m.Source.Location.X,
                        Latitude = m.Source.Location.Y,
                    }
                }
            };
        public async Task  HandleAsync(NewMatchCandidateEvent<RouteRequestAggregate> ev)
        {
            double maxDistance = configuration
                .GetValue<double>("Topology:MaxDistanceKm") * 1000d;
            int maxResults = configuration
                .GetValue<int>("Topology:MaxMatches");
            var offers = await offerRepo.GetMatch(
                ev.MatchCandidate.Source.Location, 
                ev.MatchCandidate.Destination.Location,
                ev.MatchCandidate.When, maxDistance, maxResults);
            var proposals = Enumerable.Repeat(ev.MatchCandidate, 1)
                .Select(m => PrepareMessage(m)).ToList();

            foreach (var offer in offers)
            {
                var message = new RouteExtensionProposalsMessage
                {
                    RouteId = offer.Id,
                    Proposals = proposals
                };
                queueRepo.New<RouteExtensionProposalsMessage>(message, 1);
            }
        }
    }
}
