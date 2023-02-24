using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.User;

public class ForgotPasswordRequest
{
    [Required]
    public string Email { get; set; }
}