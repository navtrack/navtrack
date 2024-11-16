using Navtrack.Api.Model.User;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.User.Mappers;

public static class UserOrganizationMapper
{
    public static UserOrganization Map(UserOrganizationElement source)
    {
        return new UserOrganization
        {
            OrganizationId = source.OrganizationId.ToString(),
            UserRole = source.UserRole
        };
    }
}