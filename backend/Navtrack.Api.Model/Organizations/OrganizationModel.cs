using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Organizations;

public class OrganizationModel
{
    [Required]
    public string Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public int AssetsCount { get; set; }

    [Required]
    public int UsersCount { get; set; }
    
    [Required]
    public int TeamsCount { get; set; }
}