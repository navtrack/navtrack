using System.ComponentModel.DataAnnotations;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Model.Assets;

public class AssetUserRoleModel
{  
    [Required]
    public AssetRoleType Role { get; set; }

    [Required]
    public string UserId { get; set; }
}