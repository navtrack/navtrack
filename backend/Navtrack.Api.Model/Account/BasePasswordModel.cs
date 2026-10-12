using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Account;

public abstract class BasePasswordModel
{
    [Required(ErrorMessage = "password.required")]
    public string Password { get; set; }

    [Required(ErrorMessage = "confirmPassword.required")]
    public string ConfirmPassword { get; set; }
}