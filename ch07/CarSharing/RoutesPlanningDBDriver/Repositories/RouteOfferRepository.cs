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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutesPlanningDBDriver.Repositories
{
    internal class RouteOfferRepository(IUnitOfWork uow) : IRouteOfferRepository
    {
        readonly MainDbContext ctx = (uow as MainDbContext)!;
        public async Task DeleteBefore(DateTime milestone)
        {
            await ctx.RouteOffers.Where(m => m.When < milestone).ExecuteDeleteAsync();
        }
        public async Task<RouteOfferAggregate?> Get(Guid id)
        {
            var entity = await ctx.RouteOffers.Where(m => m.Id == id).FirstOrDefaultAsync();
            if (entity == null) return null;
            return new RouteOfferAggregate(entity);
        }

        public async Task<IList<RouteOfferAggregate>> GetMatch(
            Point source, Point destination, 
            TimeInterval when, double distance, int maxResults)
        {
            var entities = await ctx.RouteOffers.Where(m => 
            m.Status == RouteStatus.Open &&
                m.When <= when.End && m.When >= when.Start &&
                source.Distance(m.Path) < distance &&
                destination.Distance(m.Path) < distance)
                .Select(m => new
                {
                    Distance = source.Distance(m.Path),
                    Entity = m
                })
                .OrderBy(m => m.Distance)
                .Take(maxResults).ToListAsync();
            return entities
                .Select(m => new RouteOfferAggregate(m.Entity))
                .ToList();
        }

        public RouteOfferAggregate New(Guid id, Coordinate[] path, UserBasicInfo user, DateTime When)
        {
            var entity = new RouteOffer()
            {
                Id=id,
                Path = new LineString(path) 
                    { SRID = GeometryConstants.DefaultSRID },
                When = When,
                User = user
            };
            var res= new RouteOfferAggregate(entity);
            res.AddDomainEvent(new NewMatchCandidateEvent<RouteOfferAggregate>(res));
            ctx.RouteOffers.Add(entity);
            return res;
        }
    }
}
