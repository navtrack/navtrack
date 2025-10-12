using Navtrack.Api.Model.Organizations;
using Navtrack.Database.Model.Organizations;

namespace Navtrack.Api.Services.Organizations.Mappers;

public static class OrganizationUserModelMapper
{
    public static OrganizationUserModel Map(OrganizationUserEntity source)
    {
        return new OrganizationUserModel
        {
            Email = source.User.Email,
            UserId = source.UserId.ToString(),
            UserRole = source.UserRole,
            CreatedDate = source.CreatedDate
        };
    }
}