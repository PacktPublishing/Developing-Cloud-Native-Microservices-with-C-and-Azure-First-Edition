using DDD.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutesPlanningDomainLayer.Events
{
    public class AttachedRequestEvent : IEventNotification
    {
        public IEnumerable<Guid> AddedRequests { get; set; } = null!;
        public Guid RouteOffer { get; set; } 
    }
}
