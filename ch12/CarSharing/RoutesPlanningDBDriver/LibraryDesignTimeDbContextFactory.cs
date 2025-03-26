using DBDriver;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutesPlanningDBDriver
{
    internal class LibraryDesignTimeDbContextFactory : 
        IDesignTimeDbContextFactory<MainDbContext>
    {
        private const string connectionString =
            @"Server=DESKTOP-2L6TUC9\SQLEXPRESS;Database=RoutesPlanning;User Id=sa;Password=Passw0rd_;Trust Server Certificate=True;MultipleActiveResultSets=true";
        public MainDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<MainDbContext>();
            builder.UseSqlServer(
                connectionString, 
                x => x.UseNetTopologySuite());
            return new MainDbContext(builder.Options);
        }
    }
}
