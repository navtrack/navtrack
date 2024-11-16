using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Teams;

public class Team
{
    [Required]
    public string Id { get; set; }

    [Required]
    public string Name { get; set; }
    
    [Required]
    public string OrganizationId { get; set; }

    [Required]
    public int UsersCount { get; set; }

    [Required]
    public int AssetsCount { get; set; }
}