using Navtrack.Api.Model.Organizations;
using Navtrack.Database.Model.Organizations;

namespace Navtrack.Api.Services.Organizations.Mappers;

public static class OrganizationModelMapper
{
    public static OrganizationModel Map(OrganizationEntity source, OrganizationModel? destination = null)
    {
        destination ??= new OrganizationModel();

        destination.Id = source.Id.ToString();
        destination.Name = source.Name;
        destination.AssetsCount = source.AssetsCount;
        destination.UsersCount = source.UsersCount;
        destination.TeamsCount = source.TeamsCount;

        return destination;
    }
}