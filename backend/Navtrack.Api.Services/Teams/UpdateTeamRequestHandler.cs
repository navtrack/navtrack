using System.Threading.Tasks;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Services.Teams;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Teams;

[Service(typeof(IRequestHandler<UpdateTeamRequest>))]
public class UpdateTeamRequestHandler(ITeamRepository teamRepository) : BaseRequestHandler<UpdateTeamRequest>
{
    private TeamEntity? team;

    public override async Task Validate(RequestValidationContext<UpdateTeamRequest> context)
    {
        team = await teamRepository.GetById(context.Request.TeamId);
        team.Return404IfNull();

        context.ValidationException.AddErrorIfTrue(!string.IsNullOrEmpty(context.Request.Model.Name) &&
                                                   await teamRepository.NameIsUsed(context.Request.Model.Name,
                                                       team.OrganizationId,
                                                       team.Id),
            nameof(context.Request.Model.Name),
            ApiErrorCodes.Team_000001_NameIsUsed);
    }

    public override async Task Handle(UpdateTeamRequest request)
    {
        if (!string.IsNullOrEmpty(request.Model.Name) && team!.Name != request.Model.Name)
        {
            team.Name = request.Model.Name;
            await teamRepository.Update(team);
        }
    }
}