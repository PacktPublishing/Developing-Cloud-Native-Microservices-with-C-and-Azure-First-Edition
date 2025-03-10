using DDD.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutesPlanningDomainLayer.Events
{
    public class ReleasedRequestsEvent:IEventNotification
    {
        public Guid AbortedRoute {  get; set; }
    }
}
