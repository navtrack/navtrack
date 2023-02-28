using System.ComponentModel.DataAnnotations;
using Navtrack.DataAccess.Model.Common;

namespace Navtrack.Api.Model.User;

public class UserModel
{
    [Required]
    public string Id { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public UnitsType Units { get; set; }
}