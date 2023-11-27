using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.User;

public class ForgotPasswordModel
{
    [Required]
    public string Email { get; set; }
}