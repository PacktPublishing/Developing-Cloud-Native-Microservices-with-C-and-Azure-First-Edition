using DBDriver;
using DDD.DomainLayer;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using RoutesPlanningDBDriver.Entities;
using RoutesPlanningDomainLayer;
using RoutesPlanningDomainLayer.Events;
using RoutesPlanningDomainLayer.Models.BasicTypes;
using RoutesPlanningDomainLayer.Models.Request;
using RoutesPlanningDomainLayer.Models.Route;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutesPlanningDBDriver.Repositories
{
    internal class RouteRequestRepository(IUnitOfWork uow) : IRouteRequestRepository
    {
        readonly MainDbContext ctx = (uow as MainDbContext)!;
        public async Task DeleteBefore(DateTime milestone)
        {
            await ctx.RouteRequests.Where(m => m.WhenEnd < milestone).ExecuteDeleteAsync(); 
        }

        public async Task<RouteRequestAggregate?> Get(Guid id)
        {
            var entity = await ctx.RouteRequests.Where(m => m.Id == id).FirstOrDefaultAsync();
            if (entity == null) return null;
            return new RouteRequestAggregate(entity);
        }
        public async Task<IList<RouteRequestAggregate>> Get(Guid[] ids)
        {
            var entities = await ctx.RouteRequests.Where(m => ids.Contains(m.Id) ).ToListAsync();
            
            return entities.Select(m => new RouteRequestAggregate(m)).ToList();
        }

        public async Task<IList<RouteRequestAggregate>> GetInRoute(Guid routeId)
        {
            var entities = await ctx.RouteRequests.Where(m => m.RouteId == routeId).ToListAsync();
            return entities.Select(m => new RouteRequestAggregate(m)).ToList();
        }

        public async Task<IList<RouteRequestAggregate>> GetMatch(
            IEnumerable<Coordinate> geometry, DateTime when, 
            double distance, int maxResults)
        {
            var lineString = new LineString(geometry.ToArray())
                { SRID = GeometryConstants.DefaultSRID };
            var entities = await ctx.RouteRequests.Where(m =>
                m.RouteId == null &&
                when <= m.WhenEnd && when >= m.WhenStart &&
                lineString.Distance(m.Source.Location) < distance &&
                lineString.Distance(m.Destination.Location) < distance)
                .Select(m => new
                {
                    Distance = lineString.Distance(m.Source.Location),
                    Entity = m
                })
                .OrderBy(m => m.Distance)
                .Take(maxResults).ToListAsync();
            return entities
               .Select(m => new RouteRequestAggregate(m.Entity))
               .ToList();
        }

        public RouteRequestAggregate New(Guid id, TownBasicInfo source, TownBasicInfo destination, TimeInterval when, UserBasicInfo user)
        {
            var entity = new RouteRequest()
            {
                Id = id,
                Source = source,
                Destination = destination,
                WhenStart = when.Start,
                WhenEnd = when.End,
                User = user
            };

            var res = new RouteRequestAggregate(entity);
            res.AddDomainEvent(new NewMatchCandidateEvent<RouteRequestAggregate>(res));
            ctx.RouteRequests.Add(entity);
            return res;
        }
    }
}
