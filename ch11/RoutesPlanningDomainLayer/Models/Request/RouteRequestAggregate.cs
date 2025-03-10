using DDD.DomainLayer;
using RoutesPlanningDomainLayer.Models.BasicTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutesPlanningDomainLayer.Models.Request
{
    public class RouteRequestAggregate(IRouteRequestState state): 
        Entity<Guid>
    {
        public override Guid Id => state.Id;
        public TownBasicInfo Source => state.Source;
        public TownBasicInfo Destination => state.Destination;
        TimeInterval _When = null!;
        public TimeInterval When => _When ?? 
            (_When=new TimeInterval {Start = state.WhenStart, End = state.WhenEnd });    
        public UserBasicInfo User => state.User;
        public bool Open => state.RouteId == null; 
        public long TimeStamp => state.TimeStamp;
        public void DetachFromRoute() => state.RouteId = null;
        public void AttachToRoute(Guid routeId) => state.RouteId = routeId;
    }
}
