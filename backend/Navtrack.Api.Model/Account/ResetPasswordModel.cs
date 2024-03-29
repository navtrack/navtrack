using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Account;

public class ResetPasswordModel : BasePasswordModel
{
    [Required]
    public string Hash { get; set; }
}