using System.ComponentModel.DataAnnotations;

namespace Navtrack.Api.Model.Organizations;

public class CreateOrganizationModel
{
    [Required(ErrorMessage = "name.required")]
    public string Name { get; set; }
}