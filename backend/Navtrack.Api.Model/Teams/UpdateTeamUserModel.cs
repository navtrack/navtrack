using System.ComponentModel.DataAnnotations;
using Navtrack.Database.Model.Teams;

namespace Navtrack.Api.Model.Teams;

public class UpdateTeamUserModel
{
    [Required]
    public TeamUserRole UserRole { get; set; }
}