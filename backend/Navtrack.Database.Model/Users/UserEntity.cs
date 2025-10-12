using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Model.Shared;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Postgres;

namespace Navtrack.Database.Model.Users;

[Table("users")]
public class UserEntity : BaseEntity
{
    [StringLength(255)]
    public string Email { get; set; }

    public DateTime CreatedDate { get; set; }
    
    [StringLength(88)]
    public string? PasswordHash { get; set; }
    
    [StringLength(44)]
    public string? PasswordSalt { get; set; }

    public UnitsType UnitsType { get; set; }

    public string? OldId { get; set; }
    
    public List<OrganizationEntity> Organizations { get; set; } = [];
    public List<OrganizationUserEntity> OrganizationUsers { get; set; } = [];
    public List<TeamEntity> Teams { get; set; } = [];
    public List<TeamUserEntity> TeamUsers { get; set; } = [];
    public List<AssetEntity> Assets { get; set; } = [];
    public List<AssetUserEntity> AssetUsers { get; set; } = [];
}