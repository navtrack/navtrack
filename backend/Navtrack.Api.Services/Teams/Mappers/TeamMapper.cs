using Navtrack.Api.Model.Teams;
using Navtrack.Database.Model.Teams;

namespace Navtrack.Api.Services.Teams.Mappers;

public static class TeamMapper
{
    public static Team Map(TeamEntity source)
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