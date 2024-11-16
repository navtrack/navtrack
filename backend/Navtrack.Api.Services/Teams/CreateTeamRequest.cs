using Navtrack.Api.Model.Teams;

namespace Navtrack.Api.Services.Teams;

public class CreateTeamRequest
{
    public string OrganizationId { get; set; }
    public CreateTeam Model { get; set; }
}