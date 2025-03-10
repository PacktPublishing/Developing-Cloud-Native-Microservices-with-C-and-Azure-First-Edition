using DDD.DomainLayer;
using NetTopologySuite.Geometries;
using RoutesPlanningDomainLayer.Events;
using RoutesPlanningDomainLayer.Models.BasicTypes;
using RoutesPlanningDomainLayer.Models.Request;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

namespace RoutesPlanningDomainLayer.Models.Route
{
    public class RouteOfferAggregate
        (IRouteOfferState state): Entity<Guid>
    {
        public override Guid Id => state.Id;
        IReadOnlyList<Coordinate>? _Path=null;
        public IReadOnlyList<Coordinate> Path => _Path != null ? _Path : (_Path = state.Path.Coordinates.ToImmutableList());
        public DateTime When => state.When;
        public UserBasicInfo User => state.User;
        public RouteStatus Status => state.Status;
        public long TimeStamp => state.TimeStamp;
        public void Extend(long timestamp, IEnumerable<Guid> addedRequests, Coordinate[] newRoute, bool closed)
        {
            if (timestamp > TimeStamp)
            {
                state.Path = new LineString(newRoute)
                    { SRID = GeometryConstants.DefaultSRID };
                _Path = null;
                state.TimeStamp = timestamp;
            }
            if(state.Status != RouteStatus.Aborted)
                AddDomainEvent(new AttachedRequestEvent { 
                    AddedRequests = addedRequests,
                    RouteOffer = Id
                });
            Close();
        }
        public void Close()
        {
            state.Status = RouteStatus.Closed;
        }
        public void Abort()
        {
            state.Status = RouteStatus.Aborted;
            AddDomainEvent(new ReleasedRequestsEvent
            {
                AbortedRoute = Id
            });
        }
    }
}
