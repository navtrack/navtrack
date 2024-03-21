using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Account;

public abstract class BasePasswordModel
{
    [Required]
    public string Password { get; set; }

    [Required]
    public string ConfirmPassword { get; set; }
}