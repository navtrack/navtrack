using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Postgres;

namespace Navtrack.Database.Model.Teams;

[Table("teams")]
public class TeamEntity : BaseEntity
{
    public Guid Guid { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid OrganizationId { get; set; }
    public OrganizationEntity Organization { get; set; }

    public int AssetsCount { get; set; }
    
    public int UsersCount { get; set; }

    public string? OldId { get; set; }
    
    public List<UserEntity> Users { get; set; } = [];
    public List<AssetEntity> Assets { get; set; } = [];
}