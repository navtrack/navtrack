using Navtrack.Api.Model.Teams;
using Navtrack.DataAccess.Model.Teams;

namespace Navtrack.Api.Services.Teams.Mappers;

public static class TeamMapper
{
    public static Team Map(TeamDocument source)
    {
        return new Team
        {
            Id = source.Id.ToString(),
            Name = source.Name,
            OrganizationId = source.OrganizationId.ToString(),
            UsersCount = source.UsersCount,
            AssetsCount = source.AssetsCount
        };
    }
}