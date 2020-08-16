using Microsoft.EntityFrameworkCore;

namespace Navtrack.DataAccess.Model
{
    public class NavtrackDbContext : DbContext
    {
        public NavtrackDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<AssetEntity> Assets { get; set; }
        public DbSet<LocationEntity> Locations { get; set; }
        public DbSet<DeviceEntity> Devices { get; set; }
        public DbSet<DeviceConnectionEntity> DeviceConnections { get; set; }
        // public DbSet<DeviceConnectionMessageEntity> DeviceConnectionMessages { get; set; }
        public DbSet<LogEntity> Logs { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ConfigurationEntity> Configurations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocationEntity>(entity =>
            {
                entity.ToTable("Locations");

                entity.HasKey(x => x.Id);
                entity.Property(x => x.Latitude).HasColumnType("decimal(9, 6)").IsRequired();
                entity.Property(x => x.Longitude).HasColumnType("decimal(9, 6)").IsRequired();
                entity.Property(x => x.DateTime).IsRequired();
                entity.Property(x => x.Speed).HasColumnType("decimal(6, 2)");
                entity.Property(x => x.Heading).HasColumnType("decimal(5, 2)");
                entity.Property(x => x.Altitude).HasColumnType("decimal(7, 2)");
                entity.Property(x => x.HDOP).HasColumnType("decimal(4, 2)");
                entity.Property(x => x.DeviceId).IsRequired();
                entity.Property(x => x.AssetId).IsRequired();

                entity.HasOne(x => x.Device)
                    .WithMany(x => x.Locations)
                    .HasForeignKey(x => x.DeviceId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.Asset)
                    .WithMany(x => x.Locations)
                    .HasForeignKey(x => x.AssetId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<DeviceEntity>(entity =>
            {
                entity.ToTable("Devices");

                entity.HasKey(x => x.Id);

                entity.HasMany(x => x.Locations)
                    .WithOne(x => x.Device)
                    .HasForeignKey(x => x.DeviceId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.Asset)
                    .WithMany(x => x.Devices)
                    .HasForeignKey(x => x.AssetId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<AssetEntity>(entity =>
            {
                entity.ToTable("Assets");

                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).HasMaxLength(200).IsRequired();

                entity.HasMany(x => x.Locations)
                    .WithOne(x => x.Asset)
                    .HasForeignKey(x => x.AssetId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(x => x.Devices)
                    .WithOne(x => x.Asset)
                    .HasForeignKey(x => x.AssetId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<DeviceConnectionEntity>(entity =>
            {
                entity.ToTable("DeviceConnections");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.RemoteEndPoint)
                    .HasMaxLength(64)
                    .IsRequired();
                
                entity.HasMany(x => x.Messages)
                    .WithOne(x => x.Connection)
                    .HasForeignKey(x => x.DeviceConnectionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<DeviceConnectionMessageEntity>(entity =>
            {
                entity.ToTable("DeviceConnectionMessages");
                entity.HasKey(x => x.Id);
                
                entity.HasOne(x => x.Connection)
                    .WithMany(x => x.Messages)
                    .HasForeignKey(x => x.DeviceConnectionId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
            
            modelBuilder.Entity<LogEntity>(entity =>
            {
                entity.ToTable("Logs");
                entity.HasKey(x => x.Id);
            });

            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Email).HasMaxLength(254).IsRequired();
                entity.Property(x => x.Salt).HasMaxLength(44).IsRequired();
                entity.Property(x => x.Hash).HasMaxLength(88).IsRequired();
                entity.Property(x => x.Role).IsRequired();
            });

            modelBuilder.Entity<UserAssetEntity>(entity =>
            {
                entity.ToTable("UserAsset");
                entity.HasKey(t => new {t.UserId, t.AssetId});

                entity.HasOne(pt => pt.User)
                    .WithMany(p => p.Assets)
                    .HasForeignKey(pt => pt.UserId);

                entity.HasOne(pt => pt.Asset)
                    .WithMany(t => t.Users)
                    .HasForeignKey(pt => pt.AssetId);
            });

            modelBuilder.Entity<ConfigurationEntity>(entity =>
            {
                entity.ToTable("Configurations");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Key).IsRequired().HasMaxLength(500);
                entity.Property(x => x.Value).IsRequired();
            });
        }
    }
}