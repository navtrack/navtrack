using Microsoft.EntityFrameworkCore;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Authentication;
using Navtrack.Database.Model.Devices;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Model.Users;

namespace Navtrack.Database.Model;

public abstract class BaseNavtrackDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<AssetEntity> Assets { get; set; }
    public DbSet<AssetUserEntity> AssetUsers { get; set; }
    public DbSet<AuthRefreshTokenEntity> AuthRefreshTokens { get; set; }
    public DbSet<DeviceConnectionEntity> DeviceConnections { get; set; }

    public DbSet<DeviceConnectionDataEntity> DeviceConnectionData { get; set; }
    public DbSet<DeviceEntity> Devices { get; set; }
    public DbSet<DeviceMessageEntity> DeviceMessages { get; set; }
    public DbSet<OrganizationEntity> Organizations { get; set; }
    public DbSet<OrganizationUserEntity> OrganizationUsers { get; set; }
    public DbSet<SystemEventEntity> SystemEvents { get; set; }
    public DbSet<SystemSettingEntity> SystemSettings { get; set; }
    public DbSet<TeamAssetEntity> TeamAssets { get; set; }
    public DbSet<TeamEntity> Teams { get; set; }
    public DbSet<TeamUserEntity> TeamUsers { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<UserPasswordResetEntity> UserPasswordResets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AssetEntity>(entity =>
        {
            entity.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

            entity.HasMany(x => x.Users)
                .WithMany(x => x.Assets)
                .UsingEntity<AssetUserEntity>();

            entity
                .HasOne(x => x.Device)
                .WithOne()
                .HasForeignKey<AssetEntity>(x => x.DeviceId)
                .IsRequired(false);

            entity
                .HasOne(x => x.LastMessage)
                .WithOne()
                .HasForeignKey<AssetEntity>(x => x.LastMessageId)
                .IsRequired(false);

            entity
                .HasOne(x => x.LastPositionMessage)
                .WithOne()
                .HasForeignKey<AssetEntity>(x => x.LastPositionMessageId)
                .IsRequired(false);
        });

        modelBuilder.Entity<AssetUserEntity>(entity =>
        {
            entity.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

            entity.HasAlternateKey(x => new { x.AssetId, x.UserId });
        });

        modelBuilder.Entity<AuthRefreshTokenEntity>(entity =>
        {
            entity.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");
        });

        modelBuilder.Entity<DeviceConnectionEntity>(entity =>
        {
            entity.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

            entity.HasMany(x => x.Data)
                .WithOne(x => x.Connection)
                .HasForeignKey(x => x.ConnectionId);
        });

        modelBuilder.Entity<DeviceConnectionDataEntity>(entity =>
        {
            entity.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");
        });

        modelBuilder.Entity<DeviceEntity>(entity =>
        {
            entity.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");
        });

        modelBuilder.Entity<DeviceMessageEntity>(entity =>
        {
            entity.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

            entity.Property(x => x.AdditionalData)
                .HasColumnType("jsonb");

            entity.HasIndex(x => x.AssetId);

            entity.HasIndex(e => new { e.AssetId, e.Date });
        });

        modelBuilder.Entity<OrganizationEntity>(entity =>
        {
            entity.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

            entity.HasMany(x => x.Users)
                .WithMany(x => x.Organizations)
                .UsingEntity<OrganizationUserEntity>();
        });

        modelBuilder.Entity<OrganizationUserEntity>(entity =>
        {
            entity.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

            entity.HasAlternateKey(x => new { x.OrganizationId, x.UserId });
        });

        modelBuilder.Entity<SystemEventEntity>(entity =>
        {
            entity.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

            entity.Property(x => x.Payload).HasColumnType("jsonb");
        });

        modelBuilder.Entity<SystemSettingEntity>(entity =>
        {
            entity.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

            entity.Property(x => x.Value)
                .HasColumnType("jsonb");
        });

        modelBuilder.Entity<TeamAssetEntity>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");

            entity.HasAlternateKey(x => new { x.TeamId, x.AssetId });
        });

        modelBuilder.Entity<TeamEntity>(entity =>
        {
            entity.Property(x => x.Guid).HasDefaultValueSql("gen_random_uuid()");

            entity.HasMany(x => x.Assets)
                .WithMany(x => x.Teams)
                .UsingEntity<TeamAssetEntity>();

            entity.HasMany(x => x.Users)
                .WithMany(x => x.Teams)
                .UsingEntity<TeamUserEntity>();
        });

        modelBuilder.Entity<TeamUserEntity>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");

            entity.HasAlternateKey(x => new { x.TeamId, x.UserId });
        });

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

            entity.HasMany(x => x.Organizations)
                .WithMany(x => x.Users)
                .UsingEntity<OrganizationUserEntity>();
        });

        modelBuilder.Entity<UserPasswordResetEntity>(entity =>
        {
            entity.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");
        });
    }
}