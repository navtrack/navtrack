using System.ComponentModel.DataAnnotations;
using Navtrack.Database.Model.Teams;

namespace Navtrack.Api.Model.Teams;

public class CreateTeamUserModel
{
    [Required(ErrorMessage = "email.required")]
    public string Email { get; set; }

    [Required(ErrorMessage = "userRole.required")]
    public TeamUserRole UserRole { get; set; }
}