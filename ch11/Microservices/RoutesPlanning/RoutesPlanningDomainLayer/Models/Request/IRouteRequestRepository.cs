using DDD.DomainLayer;
using NetTopologySuite.Geometries;
using RoutesPlanningDomainLayer.Models.BasicTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutesPlanningDomainLayer.Models.Request
{
    public interface IRouteRequestRepository : IRepository
    {
        RouteRequestAggregate New(
            Guid id,
            TownBasicInfo source, 
            TownBasicInfo destination,
            TimeInterval when,
            UserBasicInfo user
            );
        Task<RouteRequestAggregate?> Get(Guid id);
        Task<IList<RouteRequestAggregate>> Get(Guid[] ids);
        Task<IList<RouteRequestAggregate>> GetInRoute(Guid routeId);
        Task<IList<RouteRequestAggregate>> GetMatch(IEnumerable<Coordinate> geometry, DateTime when, double distance, int maxResults);
        Task DeleteBefore(DateTime milestone);
    }
}
