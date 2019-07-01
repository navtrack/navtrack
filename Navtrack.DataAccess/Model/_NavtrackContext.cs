using Microsoft.EntityFrameworkCore;

namespace Navtrack.DataAccess.Model
{
    public class NavtrackContext : DbContext
    {
        public NavtrackContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Location> Locations { get; set; }
        public DbSet<Device> Devices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Latitude)
                    .IsRequired();
            
                entity.Property(x => x.Longitude)
                    .IsRequired();
                
                entity.Property(x => x.DateTime)
                    .IsRequired();

                entity.HasOne(x => x.Device)
                    .WithMany(x => x.Locations)
                    .HasForeignKey(x => x.DeviceId);
                
                entity.HasOne(x => x.Asset)
                    .WithMany(x => x.Locations)
                    .HasForeignKey(x => x.AssetId);
            });
            
            modelBuilder.Entity<Device>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.IMEI)
                    .IsRequired();

                entity.HasMany(x => x.Locations)
                    .WithOne(x => x.Device)
                    .HasForeignKey(x => x.DeviceId);
            });
            
            modelBuilder.Entity<Asset>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired();

                entity.HasMany(x => x.Locations)
                    .WithOne(x => x.Asset)
                    .HasForeignKey(x => x.AssetId);
            });
        }
    }
}