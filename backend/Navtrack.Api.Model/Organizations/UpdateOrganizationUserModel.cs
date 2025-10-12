using System.ComponentModel.DataAnnotations;
using Navtrack.Database.Model.Organizations;

namespace Navtrack.Api.Model.Organizations;

public class UpdateOrganizationUserModel
{
    [Required]
    public OrganizationUserRole UserRole { get; set; }
}