using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Organizations;

public class CreateOrganizationModel
{
    [Required]
    public string Name { get; set; }
}