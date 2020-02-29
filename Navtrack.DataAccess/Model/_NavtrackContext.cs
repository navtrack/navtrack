using Microsoft.EntityFrameworkCore;

namespace Navtrack.DataAccess.Model
{
    public class NavtrackContext : DbContext
    {
        public NavtrackContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<DeviceType> DeviceTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Configuration> Configurations { get; set; }

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

                entity.HasOne(x => x.Asset)
                    .WithMany(x => x.Locations)
                    .HasForeignKey(x => x.AssetId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Device>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.IMEI).HasMaxLength(15).IsRequired();
                entity.Property(x => x.Name).HasMaxLength(200).IsRequired();

                entity.HasMany(x => x.Locations)
                    .WithOne(x => x.Device)
                    .HasForeignKey(x => x.DeviceId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.Asset)
                    .WithOne(x => x.Device)
                    .HasForeignKey<Asset>(x => x.DeviceId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.DeviceType)
                    .WithMany(x => x.Devices)
                    .HasForeignKey(x => x.DeviceTypeId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Asset>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
                entity.Property(x => x.DeviceId).IsRequired();

                entity.HasMany(x => x.Locations)
                    .WithOne(x => x.Asset)
                    .HasForeignKey(x => x.AssetId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.Device)
                    .WithOne(x => x.Asset)
                    .HasForeignKey<Asset>(x => x.DeviceId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Connection>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.RemoteEndPoint)
                    .HasMaxLength(64)
                    .IsRequired();
            });

            modelBuilder.Entity<Log>(entity => { entity.HasKey(x => x.Id); });

            modelBuilder.Entity<DeviceType>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Brand).HasMaxLength(200).IsRequired();
                entity.Property(x => x.Model).HasMaxLength(200).IsRequired();
                entity.Property(x => x.ProtocolId).IsRequired();

                entity.HasMany(x => x.Devices)
                    .WithOne(x => x.DeviceType)
                    .HasForeignKey(x => x.DeviceTypeId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Email).HasMaxLength(254).IsRequired();
                entity.Property(x => x.Salt).HasMaxLength(44).IsRequired();
                entity.Property(x => x.Hash).HasMaxLength(88).IsRequired();
            });

            modelBuilder.Entity<UserAsset>()
                .HasKey(t => new {t.UserId, t.AssetId});

            modelBuilder.Entity<UserAsset>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.Assets)
                .HasForeignKey(pt => pt.UserId);

            modelBuilder.Entity<UserAsset>()
                .HasOne(pt => pt.Asset)
                .WithMany(t => t.Users)
                .HasForeignKey(pt => pt.AssetId);


            modelBuilder.Entity<UserDevice>()
                .HasKey(t => new {t.UserId, t.DeviceId});

            modelBuilder.Entity<UserDevice>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.Devices)
                .HasForeignKey(pt => pt.UserId);

            modelBuilder.Entity<UserDevice>()
                .HasOne(pt => pt.Device)
                .WithMany(t => t.Users)
                .HasForeignKey(pt => pt.DeviceId);

            modelBuilder.Entity<Configuration>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Key).IsRequired().HasMaxLength(500);
                entity.Property(x => x.Value).IsRequired();
            });
        }
    }
}