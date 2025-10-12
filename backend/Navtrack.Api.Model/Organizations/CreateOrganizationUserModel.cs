using System.ComponentModel.DataAnnotations;
using Navtrack.Database.Model.Organizations;

namespace Navtrack.Api.Model.Organizations;

public class CreateOrganizationUserModel
{
    [Required]
    public string Email { get; set; }

    [Required]
    public OrganizationUserRole UserRole { get; set; }
}