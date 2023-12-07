using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Assets;

public class CreateAssetUserModel
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Role { get; set; }
}