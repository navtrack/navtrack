using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Account;

public class ForgotPasswordModel
{
    [Required]
    public string Email { get; set; }
}