using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Teams;

public class CreateTeam
{
    [Required]
    public string Name { get; set; }
}