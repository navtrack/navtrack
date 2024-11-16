using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Teams;

public class CreateTeamAsset
{
    [Required]
    public string AssetId { get; set; }
}