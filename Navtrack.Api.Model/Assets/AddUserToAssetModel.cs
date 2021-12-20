using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Assets;

public class AddUserToAssetModel
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Role { get; set; }
}