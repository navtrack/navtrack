using System.Threading.Tasks;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Services.Assets;
using Navtrack.Database.Services.Organizations;
using Navtrack.Database.Services.Teams;
using Navtrack.Database.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Teams;

[Service(typeof(IRequestHandler<DeleteTeamRequest>))]
public class DeleteTeamRequestHandler(
    ITeamRepository teamRepository,
    IOrganizationRepository organizationRepository,
    IUserRepository userRepository,
    IAssetRepository assetRepository)
    : BaseRequestHandler<DeleteTeamRequest>
{
    public override async Task Handle(DeleteTeamRequest source)
    {
        TeamEntity? team = await teamRepository.GetById(source.TeamId);
        team.Return404IfNull();
        
        await teamRepository.Delete(team);
        await organizationRepository.UpdateTeamsCount(team.OrganizationId);
        await userRepository.RemoveTeamFromUsers(team.Id);
        await assetRepository.RemoveTeamFromAssets(team.Id);
    }
}
