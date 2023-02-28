using Navtrack.DataAccess.Model.Common;

namespace Navtrack.Api.Model.User;

public class UpdateUserRequest
{
    public string Email { get; set; }
    public UnitsType? UnitsType { get; set; }
}