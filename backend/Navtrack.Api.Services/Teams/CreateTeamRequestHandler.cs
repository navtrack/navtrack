using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Model.Teams;
using Navtrack.Api.Services.Common.Context;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Api.Services.Teams.Events;
using Navtrack.Api.Services.Teams.Mappers;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Services.Organizations;
using Navtrack.Database.Services.Teams;
using Navtrack.Shared.Library.DI;
using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Teams;

[Service(typeof(IRequestHandler<CreateTeamRequest, Team>))]
public class CreateTeamRequestHandler(
    ITeamRepository teamRepository,
    IOrganizationRepository organizationRepository,
    INavtrackContextAccessor navtrackContextAccessor)
    : BaseRequestHandler<CreateTeamRequest, Team>
{
    private OrganizationEntity? organization;

    public override async Task Validate(RequestValidationContext<CreateTeamRequest> context)
    {
        organization = await organizationRepository.GetById(context.Request.OrganizationId);
        organization.Return404IfNull();
        
        context.ValidationException.AddErrorIfTrue(
            await teamRepository.NameIsUsed(context.Request.Model.Name, organization.Id),
            nameof(context.Request.Model.Name),
            ApiErrorCodes.Team_000001_NameIsUsed);
    }

    public override async Task<Team> Handle(CreateTeamRequest request)
    {
        TeamEntity team = TeamEntityMapper.Map(request.Model, organization.Id,
            navtrackContextAccessor.NavtrackContext.User.Id);

        await teamRepository.Add(team);
        await organizationRepository.UpdateTeamsCount(organization!.Id);

        Team result = TeamMapper.Map(team);

        return result;
    }

    public override IEvent GetEvent(CreateTeamRequest request, Team result)
    {
        return new TeamCreatedEvent
        {
            TeamId = result.Id,
            OrganizationId = organization!.Id.ToString()
        };
    }
}