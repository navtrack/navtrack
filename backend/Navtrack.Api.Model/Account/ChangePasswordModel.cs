using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Account;

public class ChangePasswordModel : BasePasswordModel
{
    [Required]
    public string CurrentPassword { get; set; }
}