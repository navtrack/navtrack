using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Model.Teams;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Common.RequestContext;
using Navtrack.Api.Services.Requests;
using Navtrack.Api.Services.Teams.Mappers;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Services.Organizations;
using Navtrack.Database.Services.Teams;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Teams;

[Service(typeof(IRequestHandler<CreateTeamRequest, TeamModel>))]
public class CreateTeamRequestHandler(
    ITeamRepository teamRepository,
    IOrganizationRepository organizationRepository,
    INavtrackRequestContextAccessor navtrackRequestContextAccessor)
    : BaseRequestHandler<CreateTeamRequest, TeamModel>
{
    public override async Task<TeamModel> Handle(CreateTeamRequest request)
    {
        OrganizationEntity? organization = await organizationRepository.GetById(request.OrganizationId);
        organization.Return404IfNull();
        
        new ValidationApiException()
            .AddErrorIfTrue(await teamRepository.NameIsUsed(request.Model.Name, organization.Id),
                nameof(request.Model.Name), ApiErrorCodes.Team_NameIsUsed)
            .ThrowIfInvalid();
        
        TeamEntity team = TeamEntityMapper.Map(request.Model, organization.Id,
            navtrackRequestContextAccessor.NavtrackContext.CurrentUser.Id);

        await teamRepository.Add(team);
        await organizationRepository.UpdateTeamsCount(organization.Id);

        TeamModel result = TeamMapper.Map(team);

        return result;
    }
}
