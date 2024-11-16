using System.ComponentModel.DataAnnotations;
using Navtrack.DataAccess.Model.Organizations;

namespace Navtrack.Api.Model.Organizations;

public class UpdateOrganizationUser
{
    [Required]
    public OrganizationUserRole UserRole { get; set; }
}