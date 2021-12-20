using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Accounts;

public class ChangePasswordModel
{
    [Required]
    public string CurrentPassword { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string ConfirmPassword { get; set; }
}