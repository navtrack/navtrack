using System.ComponentModel.DataAnnotations;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Model.User;

public class UserAssetRoleModel
{
    [Required]
    public string AssetId { get; set; }
    
    [Required]
    public AssetRoleType Role { get; set; }
}