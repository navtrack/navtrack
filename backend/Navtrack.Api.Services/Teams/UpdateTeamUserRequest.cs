using Navtrack.Api.Model.Teams;

namespace Navtrack.Api.Services.Teams;

public class UpdateTeamUserRequest
{
    public string TeamId { get; set; }
    public string UserId { get; set; }
    public UpdateTeamUser Model { get; set; }
}