using System.ComponentModel.DataAnnotations;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Model.Assets;

public class CreateAssetUser
{
    [Required]
    public string Email { get; set; }

    [Required]
    public AssetUserRole UserRole { get; set; }
}