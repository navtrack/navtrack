using System.ComponentModel.DataAnnotations;
using Navtrack.Database.Model.Organizations;

namespace Navtrack.Api.Model.Organizations;

public class CreateOrganizationUserModel
{
    [Required(ErrorMessage = "email.required")]
    public string Email { get; set; }

    [Required(ErrorMessage = "userRole.required")]
    public OrganizationUserRole UserRole { get; set; }
}