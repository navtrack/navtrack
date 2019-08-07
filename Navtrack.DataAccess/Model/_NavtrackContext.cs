using Microsoft.EntityFrameworkCore;

namespace Navtrack.DataAccess.Model
{
    public class NavtrackContext : DbContext
    {
        public NavtrackContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Object> Objects { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Device> Devices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Latitude).HasColumnType("decimal(9, 6)").IsRequired();
                entity.Property(x => x.Longitude).HasColumnType("decimal(9, 6)").IsRequired();
                entity.Property(x => x.DateTime).IsRequired();
                entity.Property(x => x.Speed).IsRequired();
                entity.Property(x => x.Heading).IsRequired();
                entity.Property(x => x.Altitude).IsRequired();
                entity.Property(x => x.DeviceId).IsRequired();

                entity.HasOne(x => x.Device)
                    .WithMany(x => x.Locations)
                    .HasForeignKey(x => x.DeviceId)
                    .OnDelete(DeleteBehavior.NoAction);
                
                entity.HasOne(x => x.Object)
                    .WithMany(x => x.Locations)
                    .HasForeignKey(x => x.ObjectId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
            
            modelBuilder.Entity<Device>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.IMEI).HasMaxLength(15).IsRequired();

                entity.HasMany(x => x.Locations)
                    .WithOne(x => x.Device)
                    .HasForeignKey(x => x.DeviceId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Object)
                    .WithOne(x => x.Device)
                    .HasForeignKey<Object>(x => x.DeviceId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
            
            modelBuilder.Entity<Object>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
                entity.Property(x => x.DeviceId).IsRequired();

                entity.HasMany(x => x.Locations)
                    .WithOne(x => x.Object)
                    .HasForeignKey(x => x.ObjectId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Device)
                    .WithOne(x => x.Object)
                    .HasForeignKey<Object>(x => x.DeviceId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}