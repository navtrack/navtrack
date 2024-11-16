using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Account;

public class ForgotPassword
{
    [Required]
    public string Email { get; set; }
}