using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Account;

public class ResetPassword : BasePasswordModel
{
    [Required]
    public string Hash { get; set; }
}