using DDD.ApplicationLayer;
using DDD.DomainLayer;
using RoutesPlanningDomainLayer.Events;
using RoutesPlanningDomainLayer.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutesPlanningApplicationServices.EventHandlers
{
    internal class ReleasedRequestsEventHandler(
        IRouteRequestRepository repo
        ) : IEventHandler<ReleasedRequestsEvent>
    {
        public async Task HandleAsync(ReleasedRequestsEvent ev)
        {
            var requests=await repo.GetInRoute(ev.AbortedRoute);
            foreach(var request in requests) request.DetachFromRoute();
        }
    }
}
