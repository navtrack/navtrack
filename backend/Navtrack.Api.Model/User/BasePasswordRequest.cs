using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.User;

public class BasePasswordRequest
{
    [Required]
    public string Password { get; set; }

    [Required]
    public string ConfirmPassword { get; set; }
}