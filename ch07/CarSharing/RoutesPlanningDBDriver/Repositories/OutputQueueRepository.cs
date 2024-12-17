using DBDriver;
using DDD.DomainLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using NetTopologySuite.Geometries;
using RoutesPlanningDBDriver.Entities;
using RoutesPlanningDomainLayer.Models.OutputQueue;
using RoutesPlanningDomainLayer.Models.Route;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Data;

namespace RoutesPlanningDBDriver.Repositories
{
    internal class OutputQueueRepository(IUnitOfWork uow) : IOutputQueueRepository
    {
        readonly MainDbContext ctx = (uow as MainDbContext)!;
        public void Confirm(Guid[] ids)
        {
            var entities = ctx.ChangeTracker.Entries<OutputQueueItem>()
                .Where(m => ids.Contains(m.Entity.Id)).Select(m => m.Entity);
            ctx.OutputQueueItems.RemoveRange(entities);
        }

        public QueueItem New<T>(T item, int messageCode)
        {
            var entity = new OutputQueueItem()
            {
                Id = Guid.NewGuid(),
                MessageCode = messageCode,
                MessageContent = JsonSerializer.Serialize(item)
            };
            var res = new QueueItem(entity);
            ctx.OutputQueueItems.Add(entity);
            return res;
        }

        public async Task<IList<QueueItem>> Take(int N, TimeSpan requeueAfter)
        {
            List<OutputQueueItem> entities;
            using (var tx = 
                await ctx.Database.BeginTransactionAsync(IsolationLevel.Serializable))
            {
                var now = DateTime.Now;
                entities = await ctx.OutputQueueItems.Where(m => m.ReadyTime <= now)
                    .OrderBy(m => m.ReadyTime)
                    .Take(N)
                    .ToListAsync();
                if (entities.Count > 0)
                {
                    foreach (var entity in entities) 
                        { entity.ReadyTime = now + requeueAfter; }
                    await ctx.SaveChangesAsync();
                    await tx.CommitAsync();
                }
                return entities.Select(m => new QueueItem(m)).ToList();
            }
        }
    }
}
