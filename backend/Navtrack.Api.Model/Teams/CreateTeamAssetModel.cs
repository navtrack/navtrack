using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Teams;

public class CreateTeamAssetModel
{
    [Required(ErrorMessage = "assetId.required")]
    public string AssetId { get; set; }
}