using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Users;

public class UserModel
{
    [Required]
    public string Id { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public UnitsType Units { get; set; }
}