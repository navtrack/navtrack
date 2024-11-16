using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Organizations;

public class CreateOrganization
{
    [Required]
    public string Name { get; set; }
}