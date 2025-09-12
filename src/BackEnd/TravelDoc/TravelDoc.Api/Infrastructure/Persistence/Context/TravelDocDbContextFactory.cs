using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using TravelDoc.Repository.Contexts;

namespace TravelDoc.Api.Infrastructure.Persistence.Context
{
    public class TravelDocDbContextFactory : IDesignTimeDbContextFactory<TravelDocDbContext>
    {
        public TravelDocDbContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();

            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<TravelDocDbContext>();
            optionsBuilder.UseNpgsql(connectionString, options =>
            {
                options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });

            return new TravelDocDbContext(optionsBuilder.Options);
        }
    }
}
