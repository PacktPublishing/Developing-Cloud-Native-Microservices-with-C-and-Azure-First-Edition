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
        public IEnumerable<Guid> AddedRequests { get; set; } = new List<Guid>() ;
        public Guid RouteOffer { get; set; } 
    }
}
