using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Common.RequestContext;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Services.Assets;
using Navtrack.Database.Services.Teams;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Teams;

[Service(typeof(IRequestHandler<CreateTeamAssetRequest>))]
public class CreateTeamAssetRequestHandler(
    ITeamRepository teamRepository,
    IAssetRepository assetRepository,
    INavtrackRequestContextAccessor navtrackRequestContextAccessor) : BaseRequestHandler<CreateTeamAssetRequest>
{
    public override async Task Handle(CreateTeamAssetRequest request)
    {
        TeamEntity? team = await teamRepository.GetById(request.TeamId);
        team.Return404IfNull();

        AssetEntity? asset = await assetRepository.GetById(request.Model.AssetId);
        asset.Return404IfNull();

        if (team.OrganizationId != asset.OrganizationId)
        {
            throw new ApiException(ApiErrorCodes.Team_TeamAndAssetNotInSameOrganization);
        }

        new ValidationApiException()
            .AddErrorIfTrue(asset.Teams.Any(x => x.Id == team.Id), nameof(request.Model.AssetId),
                ApiErrorCodes.Team_AssetAlreadyInTeam)
            .ThrowIfInvalid();
        
        await teamRepository.AddAsset(team.Id, asset.Id, navtrackRequestContextAccessor.NavtrackContext.CurrentUser.Id);
    }
}
