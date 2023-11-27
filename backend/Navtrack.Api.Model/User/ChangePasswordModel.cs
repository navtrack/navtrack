using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.User;

public class ChangePasswordModel : BasePasswordModel
{
    [Required]
    public string CurrentPassword { get; set; }
}