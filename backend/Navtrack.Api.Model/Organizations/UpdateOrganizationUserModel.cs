using System.ComponentModel.DataAnnotations;
using Navtrack.Database.Model.Organizations;

namespace Navtrack.Api.Model.Organizations;

public class UpdateOrganizationUserModel
{
    [Required(ErrorMessage = "userRole.required")]
    public OrganizationUserRole UserRole { get; set; }
}