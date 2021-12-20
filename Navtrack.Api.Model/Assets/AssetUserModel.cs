using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Assets;

public class AssetUserModel
{
    [Required]
    public string Email { get; set; }
         
    [Required]
    public string Role { get; set; }

    [Required]
    public string UserId { get; set; }
}