using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.User;

public class ResetPasswordModel : BasePasswordModel
{
    [Required]
    public string Hash { get; set; }
}