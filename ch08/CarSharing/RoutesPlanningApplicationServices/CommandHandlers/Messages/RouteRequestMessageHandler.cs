using DDD.ApplicationLayer;
using DDD.DomainLayer;
using RoutesPlanningApplicationServices.Commands;
using RoutesPlanningDomainLayer.Models.Request;
using SharedMessages.RouteNegotiation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoutesPlanningDomainLayer.Models.BasicTypes;
using System.Xml.Linq;
using NetTopologySuite.Geometries;
using RoutesPlanningDomainLayer.Tools;
using RoutesPlanningDomainLayer;

namespace RoutesPlanningApplicationServices.CommandHandlers.Messages
{
    internal class RouteRequestMessageHandler(
        IRouteRequestRepository repo,
        IUnitOfWork uow,
        EventMediator mediator
        ) : ICommandHandler<MessageCommand<RouteRequestMessage>>
    {
        public async Task HandleAsync(MessageCommand<RouteRequestMessage> command)
        {
             var message= command.Message;
             var toCreate =repo.New(message.Id,
                new TownBasicInfo
                {
                    Id = message.Source!.Id,
                    Name = message.Source.Name!,
                    Location = new Point(message.Source.Location!.Longitude,
                        message.Source.Location!.Latitude)
                        {SRID= GeometryConstants.DefaultSRID}
                },
                new TownBasicInfo
                {
                    Id = message.Destination!.Id,
                    Name = message.Destination.Name!,
                    Location = new Point(message.Destination.Location!.Longitude,
                        message.Destination.Location!.Latitude)
                        { SRID = GeometryConstants.DefaultSRID }
                },
                new TimeInterval { Start = message.When!.Start, End = message.When!.End },
                new UserBasicInfo { Id = message.User!.Id, DisplayName = message.User.DisplayName! }
                );
            if(toCreate.DomainEvents != null && toCreate.DomainEvents.Count>0)
                await mediator.TriggerEvents( toCreate.DomainEvents );
            try
            {
                await uow.SaveEntitiesAsync();
            }
            catch (ConstraintViolationException) { }
            
        }
    }
}
