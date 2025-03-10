using DDD.DomainLayer;
using Microsoft.EntityFrameworkCore;
using RoutesPlanningDBDriver.Entities;
using RoutesPlanningDomainLayer.Models.BasicTypes;
using RoutesPlanningDomainLayer.Models.OutputQueue;
using RoutesPlanningDomainLayer.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDriver
{
    internal class MainDbContext : DbContext, IUnitOfWork
    {
        public MainDbContext(DbContextOptions options)
        : base(options)
        {
        }
        public DbSet<RouteRequest> RouteRequests { get; set; } = null!;
        public DbSet<RouteOffer> RouteOffers { get; set; } = null!;
        public DbSet<OutputQueueItem> OutputQueueItems { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder
        builder)
        {
            builder.Entity<RouteRequest>().OwnsOne(m => m.Source);
            builder.Entity<RouteRequest>().OwnsOne(m => m.Destination);
            builder.Entity<RouteRequest>().OwnsOne(m => m.User);
            builder.Entity<RouteRequest>().HasIndex(m => m.WhenStart);
            builder.Entity<RouteRequest>().HasIndex(m => m.WhenEnd);
            



            builder.Entity<RouteOffer>().OwnsOne(m => m.User);
            builder.Entity<RouteOffer>().HasMany(m => m.Requests)
                .WithOne(m => m.Route)
                .HasForeignKey(m => m.RouteId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<RouteOffer>().HasIndex(m => m.When);
            builder.Entity<RouteOffer>().HasIndex(m => m.Status);

            builder.Entity<OutputQueueItem>().HasIndex(m => m.ReadyTime);

        }
        #region IUnitOfWork Implementation
        public async Task<bool> SaveEntitiesAsync()
        {
            try
            {
                return await SaveChangesAsync() > 0;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ConcurrencyException(ex);
            }
            catch (DbUpdateException ex)
            {
                throw new ConstraintViolationException(ex);
            }
            
        }
        public async Task StartAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            await Database.BeginTransactionAsync(isolationLevel);
        }
        public Task CommitAsync()
        {
            Database.CommitTransaction();
            return Task.CompletedTask;
        }
        public Task RollbackAsync()
        {
            Database.RollbackTransaction();
            return Task.CompletedTask;
        }
        #endregion

    }
}
