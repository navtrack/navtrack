using Navtrack.Api.Model.Teams;

namespace Navtrack.Api.Services.Teams;

public class UpdateTeamRequest
{
    public string TeamId { get; set; }
    public UpdateTeam Model { get; set; }
}