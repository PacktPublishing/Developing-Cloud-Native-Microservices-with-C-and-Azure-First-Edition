using DDD.ApplicationLayer;
using DDD.DomainLayer;
using NetTopologySuite.Geometries;
using RoutesPlanningApplicationServices.Commands;
using RoutesPlanningDomainLayer.Events;
using RoutesPlanningDomainLayer.Models.BasicTypes;
using RoutesPlanningDomainLayer.Models.Route;
using SharedMessages.RouteNegotiation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutesPlanningApplicationServices.CommandHandlers.Messages
{
    internal class RouteExtendedMessageHandler(
        IRouteOfferRepository repo,
        IUnitOfWork uow,
        EventMediator mediator
        ) : ICommandHandler<MessageCommand<RouteExtendedMessage>>
    {
        public async Task HandleAsync(MessageCommand<RouteExtendedMessage> command)
        {
            var message = command.Message;
            await uow.StartAsync(System.Data.IsolationLevel.Serializable);
            try
            {
                var route = await repo.Get(message.ExtendedRoute!.Id);
                if (route is not null && route.TimeStamp != message.TimeStamp)
                {
                    route.Extend(message.TimeStamp,
                        message.AddedRequests!.Select(m => m.Id),
                        message.ExtendedRoute.Path!
                            .Select(m => new Coordinate(m.Location!.Longitude, m.Location.Latitude)).ToArray(),
                        message.Closed);
                    if (route.DomainEvents != null && route.DomainEvents.Count > 0)
                        mediator.Equals(route.DomainEvents);
                    await uow.SaveEntitiesAsync();
                    await uow.CommitAsync();
                }
                else
                {
                    await uow.RollbackAsync();
                    return;
                }
            }
            catch
            {
                await uow.RollbackAsync();
                throw;
            }
        }
    }
}
