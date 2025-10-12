using System;
using System.ComponentModel.DataAnnotations.Schema;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Postgres;

namespace Navtrack.Database.Model.Teams;

[Table("teams_assets")]
public class TeamAssetEntity : BaseEntity
{
    public Guid AssetId { get; set; }
    public AssetEntity Asset { get; set; }
    public Guid TeamId { get; set; }
    public TeamEntity Team { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid CreatedBy { get; set; }
}