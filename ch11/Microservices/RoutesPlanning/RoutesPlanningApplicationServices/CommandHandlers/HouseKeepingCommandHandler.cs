using DDD.ApplicationLayer;
using DDD.DomainLayer;
using RoutesPlanningApplicationServices.Commands;
using RoutesPlanningDomainLayer.Models.OutputQueue;
using RoutesPlanningDomainLayer.Models.Request;
using RoutesPlanningDomainLayer.Models.Route;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutesPlanningApplicationServices.CommandHandlers
{
    internal class HouseKeepingCommandHandler(
        IRouteRequestRepository requestRepo,
        IRouteOfferRepository offerRepo
        ) : ICommandHandler<HouseKeepingCommand>
    {
        public async Task HandleAsync(HouseKeepingCommand command)
        {
            var deleteTrigger = DateTime.Now.AddDays( -command.DeleteDelay );
            await offerRepo.DeleteBefore(deleteTrigger);
            await requestRepo.DeleteBefore(deleteTrigger);
        }
    }
}
