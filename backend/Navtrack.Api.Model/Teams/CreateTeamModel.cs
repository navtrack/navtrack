using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Teams;

public class CreateTeamModel
{
    [Required(ErrorMessage = "name.required")]
    public string Name { get; set; }
}