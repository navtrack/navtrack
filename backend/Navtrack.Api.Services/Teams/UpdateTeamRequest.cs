using Navtrack.Api.Model.Teams;

namespace Navtrack.Api.Services.Teams;

public class UpdateTeamRequest
{
    public string TeamId { get; set; }
    public UpdateTeamModel Model { get; set; }
}