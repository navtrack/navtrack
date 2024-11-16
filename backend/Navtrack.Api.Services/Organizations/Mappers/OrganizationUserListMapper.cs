using System.Linq;
using MongoDB.Bson;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Organizations;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Organizations.Mappers;

public static class OrganizationUserListMapper
{
    public static List<OrganizationUser> Map(System.Collections.Generic.List<UserDocument> users, ObjectId organizationId)
    {
        List<OrganizationUser> list = new()
        {
            Items = users
                .Select(x => OrganizationUserMapper.Map(x, organizationId))
                .ToList()
        };

        return list;
    }
}