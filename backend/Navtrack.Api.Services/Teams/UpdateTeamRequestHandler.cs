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
    public override async Task Handle(UpdateTeamRequest request)
    {
        TeamEntity? team = await teamRepository.GetById(request.TeamId);
        team.Return404IfNull();

        new ValidationApiException()
            .AddErrorIfTrue(!string.IsNullOrEmpty(request.Model.Name) &&
                            await teamRepository.NameIsUsed(request.Model.Name, team.OrganizationId, team.Id),
                nameof(request.Model.Name),
                ApiErrorCodes.Team_NameIsUsed)
            .ThrowIfInvalid();
        
        if (!string.IsNullOrEmpty(request.Model.Name) && team.Name != request.Model.Name)
        {
            team.Name = request.Model.Name;
            await teamRepository.Update(team);
        }
    }
}
