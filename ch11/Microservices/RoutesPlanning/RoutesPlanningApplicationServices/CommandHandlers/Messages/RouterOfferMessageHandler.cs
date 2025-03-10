using DDD.ApplicationLayer;
using DDD.DomainLayer;
using NetTopologySuite.Geometries;
using RoutesPlanningApplicationServices.Commands;
using RoutesPlanningDomainLayer.Models.BasicTypes;
using RoutesPlanningDomainLayer.Models.Route;
using RoutesPlanningDomainLayer.Tools;
using SharedMessages.RouteNegotiation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutesPlanningApplicationServices.CommandHandlers.Messages
{
    internal class RouterOfferMessageHandler(
        IRouteOfferRepository repo,
        IUnitOfWork uow,
        EventMediator mediator
        ) : ICommandHandler<MessageCommand<RouteOfferMessage>>

    {
        public async Task HandleAsync(MessageCommand<RouteOfferMessage> command)
        {
            var message = command.Message;
            var toCreate = repo.New(message.Id,
              message.Path!.Select(m => 
              new Coordinate(m.Location!.Longitude, m.Location.Latitude)).ToArray(),
               new UserBasicInfo { Id = message.User!.Id, DisplayName = message.User.DisplayName! },
               message.When!.Value
               );
            if (toCreate.DomainEvents != null && toCreate.DomainEvents.Count > 0)
                await mediator.TriggerEvents(toCreate.DomainEvents);
            try
            {
                await uow.SaveEntitiesAsync();
            }
            catch (ConstraintViolationException) { }
        }
    }
}
