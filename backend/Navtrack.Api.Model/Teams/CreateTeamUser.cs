using System.ComponentModel.DataAnnotations;
using Navtrack.Database.Model.Teams;

namespace Navtrack.Api.Model.Teams;

public class CreateTeamUser
{
    [Required]
    public string Email { get; set; }

    [Required]
    public TeamUserRole UserRole { get; set; }
}