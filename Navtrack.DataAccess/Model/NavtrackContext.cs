using Microsoft.EntityFrameworkCore;

namespace Navtrack.DataAccess.Model
{
    public class NavtrackContext : DbContext
    {
        public DbSet<Location> Locations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Location>()
                .Property(x => x.Latitude)
                .IsRequired();
            
            modelBuilder.Entity<Location>()
                .Property(x => x.Longitude)
                .IsRequired();
            
            modelBuilder.Entity<Location>()
                .Property(x => x.DateTime)
                .IsRequired();
        }
    }
}