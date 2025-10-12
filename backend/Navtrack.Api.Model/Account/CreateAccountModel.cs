using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Account;

public class CreateAccountModel
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string ConfirmPassword { get; set; }

    public string? Captcha { get; set; }
}