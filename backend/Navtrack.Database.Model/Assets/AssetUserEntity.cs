using System;
using System.ComponentModel.DataAnnotations.Schema;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Postgres;

namespace Navtrack.Database.Model.Assets;

[Table("assets_users")]
public class AssetUserEntity : BaseEntity
{
    public Guid AssetId { get; set; }
    public AssetEntity Asset { get; set; }
    public Guid UserId { get; set; }
    public UserEntity User { get; set; }
    public AssetUserRole UserRole { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
}