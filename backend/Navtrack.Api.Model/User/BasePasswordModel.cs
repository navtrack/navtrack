using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.User;

public class BasePasswordModel
{
    [Required]
    public string Password { get; set; }

    [Required]
    public string ConfirmPassword { get; set; }
}