using DDD.ApplicationLayer;
using RoutesPlanningDomainLayer.Events;
using RoutesPlanningDomainLayer.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutesPlanningApplicationServices.EventHandlers
{
    internal class AttachedRequestEventHandler(
        IRouteRequestRepository repo
        ) : IEventHandler<AttachedRequestEvent>
    {
        public async Task HandleAsync(AttachedRequestEvent ev)
        {
            var requests = await repo.Get(ev.AddedRequests.ToArray());
            foreach (var request in requests) request.AttachToRoute(ev.RouteOffer);
        }
    }
}
