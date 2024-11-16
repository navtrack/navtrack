using Navtrack.Api.Model.Teams;

namespace Navtrack.Api.Services.Teams;

public class CreateTeamUserRequest
{
    public string TeamId { get; set; }
    public CreateTeamUser Model { get; set; }
}