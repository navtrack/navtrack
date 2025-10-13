using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Navtrack.Database.Model;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<NavtrackDbContext>
{
    public NavtrackDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<NavtrackDbContext> optionsBuilder = new();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=navtrack;Username=navtrack;Password=navtrack");
        
        return new NavtrackDbContext(optionsBuilder.Options);
    }
}