using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Devices;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Postgres;

namespace Navtrack.Database.Model.Organizations;

[Table("organizations")]
public class OrganizationEntity : BaseEntity
{
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid CreatedBy { get; set; }
    public int AssetsCount { get; set; }
    public int UsersCount { get; set; }
    public int TeamsCount { get; set; }
    public int DevicesCount { get; set; }
    public List<TeamEntity> Teams { get; set; } = [];
    public List<DeviceEntity> Devices { get; set; } = [];
    public List<AssetEntity> Assets { get; set; } = [];
    public List<UserEntity> Users { get; set; } = [];
    public List<OrganizationUserEntity> OrganizationUsers { get; set; } = [];
    public string? OldId { get; set; }
}