using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Account;

public class CreateAccountModel
{
    [Required(ErrorMessage = "email.required")]
    public string Email { get; set; }

    [Required(ErrorMessage = "password.required")]
    public string Password { get; set; }

    [Required(ErrorMessage = "confirmPassword.required")]
    public string ConfirmPassword { get; set; }

    public string? Captcha { get; set; }
}