using System.ComponentModel.DataAnnotations;
using Navtrack.DataAccess.Model.Organizations;

namespace Navtrack.Api.Model.Organizations;

public class CreateOrganizationUser
{
    [Required]
    public string Email { get; set; }

    [Required]
    public OrganizationUserRole UserRole { get; set; }
}