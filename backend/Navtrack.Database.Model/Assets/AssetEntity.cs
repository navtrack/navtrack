using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Navtrack.Database.Model.Devices;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Model.Shared;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Postgres;

namespace Navtrack.Database.Model.Assets;

[Table("assets")]
public class AssetEntity : BaseEntity
{
    public string Name { get; set; }
    public Guid OrganizationId { get; set; }
    public OrganizationEntity Organization { get; set; }
    public Guid? DeviceId { get; set; }
    public DeviceEntity? Device { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid CreatedBy { get; set; }
    public string? OldId { get; set; }
    public List<TeamEntity> Teams { get; set; } = [];
    public List<TeamAssetEntity> TeamAssets { get; set; } = [];
    public List<UserEntity> Users { get; set; } = [];
    public List<AssetUserEntity> AssetUsers { get; set; } = [];
    public List<DeviceEntity> Devices { get; set; } = [];
    public List<DeviceMessageEntity> Messages { get; set; } = [];

    public Guid? LastMessageId { get; set; }
    public DeviceMessageEntity? LastMessage { get; set; }

    public Guid? LastPositionMessageId { get; set; }
    public DeviceMessageEntity? LastPositionMessage { get; set; }
}