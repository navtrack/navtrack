using System.ComponentModel.DataAnnotations;
using Navtrack.DataAccess.Model.Shared;

namespace Navtrack.Api.Model.User;

public class UpdateUserModel
{
    [EmailAddress]
    public string? Email { get; set; }
    public UnitsType? UnitsType { get; set; }
}