using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Teams;

public class CreateTeamAssetModel
{
    [Required]
    public string AssetId { get; set; }
}