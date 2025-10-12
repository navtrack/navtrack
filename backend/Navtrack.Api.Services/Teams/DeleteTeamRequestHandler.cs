using System.Threading.Tasks;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Api.Services.Teams.Events;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Services.Assets;
using Navtrack.Database.Services.Organizations;
using Navtrack.Database.Services.Teams;
using Navtrack.Database.Services.Users;
using Navtrack.Shared.Library.DI;
using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Teams;

[Service(typeof(IRequestHandler<DeleteTeamRequest>))]
public class DeleteTeamRequestHandler(
    ITeamRepository teamRepository,
    IOrganizationRepository organizationRepository,
    IUserRepository userRepository,
    IAssetRepository assetRepository)
    : BaseRequestHandler<DeleteTeamRequest>
{
    private TeamEntity? team;

    public override async Task Validate(RequestValidationContext<DeleteTeamRequest> context)
    {
        team = await teamRepository.GetById(context.Request.TeamId);
        team.Return404IfNull();
    }

    public override async Task Handle(DeleteTeamRequest source)
    {
        await teamRepository.Delete(team!);
        await organizationRepository.UpdateTeamsCount(team!.OrganizationId);
        await userRepository.RemoveTeamFromUsers(team.Id);
        await assetRepository.RemoveTeamFromAssets(team.Id);
    }

    public override IEvent GetEvent(DeleteTeamRequest request)
    {
        return new TeamDeletedEvent
        {
            TeamId = team!.Id.ToString(),
            OrganizationId = team!.OrganizationId.ToString()
        };
    }
}