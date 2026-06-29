using System.ComponentModel.DataAnnotations;
using Navtrack.Database.Model.Shared;

namespace Navtrack.Api.Model.User;

public class UpdateUserModel
{
    [EmailAddress(ErrorMessage = "email.invalid")]
    public required string Email { get; set; }
    public UnitsType UnitsType { get; set; }
}