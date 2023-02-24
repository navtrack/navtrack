using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.User;

public class ResetPasswordRequest : BasePasswordRequest
{
    [Required]
    public string Hash { get; set; }
}