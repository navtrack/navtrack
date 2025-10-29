using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Teams;

public class CreateTeamModel
{
    [Required]
    public string Name { get; set; }
}