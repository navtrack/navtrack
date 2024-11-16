using Navtrack.Api.Model.Organizations;
using Navtrack.DataAccess.Model.Organizations;

namespace Navtrack.Api.Services.Organizations.Mappers;

public static class OrganizationMapper
{
    public static Organization Map(OrganizationDocument source, Organization? destination = null)
    {
        destination ??= new Organization();

        destination.Id = source.Id.ToString();
        destination.Name = source.Name;
        destination.AssetsCount = source.AssetsCount;
        destination.UsersCount = source.UsersCount;
        destination.TeamsCount = source.TeamsCount;

        return destination;
    }
}