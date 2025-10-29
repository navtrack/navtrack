using Navtrack.Api.Model.Teams;
using Navtrack.Database.Model.Teams;

namespace Navtrack.Api.Services.Teams.Mappers;

public static class TeamMapper
{
    public static TeamModel Map(TeamEntity source)
    {
        return new TeamModel
        {
            Id = source.Id.ToString(),
            Name = source.Name,
            OrganizationId = source.OrganizationId.ToString(),
            UsersCount = source.UsersCount,
            AssetsCount = source.AssetsCount
        };
    }
}