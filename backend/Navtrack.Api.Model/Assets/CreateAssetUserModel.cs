using System.ComponentModel.DataAnnotations;
using Navtrack.Database.Model.Assets;

namespace Navtrack.Api.Model.Assets;

public class CreateAssetUserModel
{
    [Required(ErrorMessage = "email.required")]
    public string Email { get; set; }

    [Required(ErrorMessage = "userRole.required")]
    public AssetUserRole UserRole { get; set; }
}