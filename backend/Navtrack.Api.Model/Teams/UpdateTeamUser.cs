using System.ComponentModel.DataAnnotations;
using Navtrack.DataAccess.Model.Teams;

namespace Navtrack.Api.Model.Teams;

public class UpdateTeamUser
{
    [Required]
    public TeamUserRole UserRole { get; set; }
}