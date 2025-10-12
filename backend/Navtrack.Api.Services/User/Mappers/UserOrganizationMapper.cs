using Navtrack.Api.Model.User;
using Navtrack.Database.Model.Organizations;

namespace Navtrack.Api.Services.User.Mappers;

public static class UserOrganizationMapper
{
    public static UserOrganizationModel Map(OrganizationUserEntity source)
    {
        return new UserOrganizationModel
        {
            OrganizationId = source.OrganizationId.ToString(),
            UserRole = source.UserRole
        };
    }
}