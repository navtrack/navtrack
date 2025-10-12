using System.ComponentModel.DataAnnotations;
using Navtrack.Database.Model.Assets;

namespace Navtrack.Api.Model.Assets;

public class CreateAssetUserModel
{
    [Required]
    public string Email { get; set; }

    [Required]
    public AssetUserRole UserRole { get; set; }
}