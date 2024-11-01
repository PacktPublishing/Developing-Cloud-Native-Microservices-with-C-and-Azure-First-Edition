using DDD.DomainLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        protected override void OnModelCreating(ModelBuilder
        builder)
        {
        }
        #region IUnitOfWork Implementation
        public async Task<bool> SaveEntitiesAsync()
        {
            return await SaveChangesAsync() > 0; ;
        }
        public async Task StartAsync()
        {
            await Database.BeginTransactionAsync();
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
