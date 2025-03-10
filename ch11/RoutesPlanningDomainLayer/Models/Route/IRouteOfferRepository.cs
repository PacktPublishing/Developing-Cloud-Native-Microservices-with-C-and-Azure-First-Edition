using DDD.DomainLayer;
using NetTopologySuite.Geometries;
using RoutesPlanningDomainLayer.Models.BasicTypes;
using RoutesPlanningDomainLayer.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutesPlanningDomainLayer.Models.Route
{
    public interface IRouteOfferRepository : IRepository
    {
        RouteOfferAggregate New(Guid id, Coordinate[] path, UserBasicInfo user, DateTime When);
        Task<RouteOfferAggregate?> Get(Guid id);
        Task<IList<RouteOfferAggregate>> GetMatch(Point source, Point destination, TimeInterval when, double distance, int maxResults);
        Task DeleteBefore(DateTime milestone);
    }
}
