using System.Linq;
using MongoDB.Bson;
using Navtrack.Api.Model.Organizations;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Organizations.Mappers;

public static class OrganizationUserMapper
{
    public static OrganizationUser Map(UserDocument user, ObjectId source)
    {
        UserOrganizationElement userOrganization = user.Organizations!.First(x => x.OrganizationId == source);

        return new OrganizationUser
        {
            Email = user.Email,
            UserId = user.Id.ToString(),
            UserRole = userOrganization.UserRole,
            CreatedDate = userOrganization.CreatedDate
        };
    }
}