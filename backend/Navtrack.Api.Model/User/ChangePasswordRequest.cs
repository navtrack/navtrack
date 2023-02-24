using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.User;

public class ChangePasswordRequest : BasePasswordRequest
{
    [Required]
    public string CurrentPassword { get; set; }
}